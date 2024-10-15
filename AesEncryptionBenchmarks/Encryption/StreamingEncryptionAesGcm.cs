using System.Buffers;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace AesEncryptionBenchmarks.Encryption;

public sealed class StreamingEncryptionAesGcm(ReadOnlySpan<byte> key) : IDisposable
{
    readonly AesGcm _aesGcm = new(key, AesGcmConstants.TagSize);

    public async Task DecryptAsync(Stream input, Stream output, CancellationToken cancellationToken) 
    {
        byte[] inputBuffer = null!;
        byte[] outputBuffer = null!;
        try
        {
            var streamLength = input.Length;
            var data = new byte[4];
            // read QEv1
            await input.ReadExactlyAsync(data, cancellationToken);
            if (!data.AsSpan().SequenceEqual(AesGcmConstants.Qev1.Span))
                throw new CryptographicException("Encryption hash mismatch");
            
            // read chunk size
            await input.ReadExactlyAsync(data, cancellationToken);
            var chunkSize = MemoryMarshal.Read<int>(data);
            if (chunkSize > AesGcmConstants.MaxChunkSize)
                throw new CryptographicException($"Chunk size is too big: {chunkSize}");
            
            var chunkSizeForPlainText = chunkSize - AesGcmConstants.NonceSize - AesGcmConstants.TagSize;
            inputBuffer = ArrayPool<byte>.Shared.Rent(chunkSize);
            outputBuffer = ArrayPool<byte>.Shared.Rent(chunkSizeForPlainText);
            var inputMemory = inputBuffer.AsMemory(0, chunkSize);
            
            var terminalChunkIndex = (streamLength - 4/*QEv1*/ - 4 /*chunk size*/) / chunkSize - 1 + ((streamLength - 4/*QEv1*/ - 4 /*chunk size*/) % chunkSize == 0 ? 0 : 1);
            int chunkIndex = 0;
            while (true)
            {
                var isTerminal = terminalChunkIndex == chunkIndex;
                
                var read = await input.ReadAtLeastAsync(inputMemory, chunkSize, false, cancellationToken);
                var actuallyRead = inputMemory.Slice(0, read);
                var outputMemory = outputBuffer.AsMemory(0, read - AesGcmConstants.NonceSize - AesGcmConstants.TagSize);  
                DecodeChunk(actuallyRead, outputMemory.Span, chunkIndex, isTerminal);
                
                await output.WriteAsync(outputMemory, cancellationToken);
                chunkIndex++;
                
                if (isTerminal)
                {
                    break;
                }
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(inputBuffer);
            ArrayPool<byte>.Shared.Return(outputBuffer);
        }   
    }

    void DecodeChunk(ReadOnlyMemory<byte> actuallyRead, Span<byte> outputMemory, int chunkIndex, bool isTerminal)
    {
        Span<byte> associatedData = stackalloc byte[9];
        FillAssociatedData(associatedData, chunkIndex, isTerminal);
        
        _aesGcm.Decrypt(
            actuallyRead.Span.Slice(0, AesGcmConstants.NonceSize), 
            actuallyRead.Span.Slice(AesGcmConstants.NonceSize, actuallyRead.Length - AesGcmConstants.NonceSize - AesGcmConstants.TagSize), 
            actuallyRead.Span.Slice(actuallyRead.Length - AesGcmConstants.TagSize, AesGcmConstants.TagSize), 
            outputMemory, 
            associatedData);   
    }

    static void GenerateNonce(Span<byte> data)
    {
        RandomNumberGenerator.Fill(data);
    }

    public async Task EncryptAsync(Stream input, Stream output, CancellationToken cancellationToken)
    {
        byte[] inputBuffer = null!;
        byte[] outputBuffer = null!;
        try
        {
            var streamLength = input.Length;
            var chunkSizeWithAdditionalData = (int)Math.Min(streamLength + AesGcmConstants.NonceSize + AesGcmConstants.TagSize, AesGcmConstants.MaxChunkSize);
            var readChunkSize = chunkSizeWithAdditionalData  - AesGcmConstants.NonceSize - AesGcmConstants.TagSize;
            
            inputBuffer = ArrayPool<byte>.Shared.Rent(readChunkSize);
            outputBuffer = ArrayPool<byte>.Shared.Rent(chunkSizeWithAdditionalData);
            var inputMemory = inputBuffer.AsMemory(0, readChunkSize);
            // write QEv1
            await output.WriteAsync(AesGcmConstants.Qev1, cancellationToken);

            // write chunk size
            var chunkSizeBytes = outputBuffer.AsMemory(0, 4);
            MemoryMarshal.Write(chunkSizeBytes.Span, chunkSizeWithAdditionalData);
            await output.WriteAsync(chunkSizeBytes, cancellationToken);
            
            var terminalChunkIndex = streamLength / readChunkSize - 1 + (streamLength % readChunkSize == 0 ? 0 : 1);
            int chunkIndex = 0;
            while (true)
            {
                var isTerminal = terminalChunkIndex == chunkIndex;
                
                var read = await input.ReadAtLeastAsync(inputMemory, readChunkSize, false, cancellationToken);
                var actuallyRead = inputMemory.Slice(0, read);
                
                FillOutputBuffer(
                    outputBuffer.AsSpan(0, AesGcmConstants.NonceSize), 
                    actuallyRead.Span, 
                    outputBuffer.AsSpan(AesGcmConstants.NonceSize, read), 
                    outputBuffer.AsSpan(AesGcmConstants.NonceSize + read, AesGcmConstants.TagSize), 
                    chunkIndex,  isTerminal);
                
                // chunk
                await output.WriteAsync(outputBuffer.AsMemory(0, AesGcmConstants.NonceSize + read + AesGcmConstants.TagSize), cancellationToken);
                chunkIndex++;
                
                if (isTerminal)
                {
                    break;
                }
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(inputBuffer);
            ArrayPool<byte>.Shared.Return(outputBuffer);
        }
        
    }

    void FillOutputBuffer(Span<byte> nonce, ReadOnlySpan<byte> plaintext, Span<byte> encrypted, Span<byte> tag, int chunkIndex, bool isTerminal)
    {
        GenerateNonce(nonce);
        Span<byte> associatedData = stackalloc byte[9];
        FillAssociatedData(associatedData, chunkIndex, isTerminal);
        _aesGcm.Encrypt(nonce, plaintext, encrypted, tag, associatedData);   
    }

    void FillAssociatedData(Span<byte> associatedData, int chunkIndex, bool isTerminal)
    {
        // QEv1 + chunkIndex + TerminalFlag
        // 4    + 4          + 1           = 9
        AesGcmConstants.Qev1.Span.CopyTo(associatedData);
        associatedData = associatedData[AesGcmConstants.Qev1.Length..];
        
        MemoryMarshal.Write(associatedData, chunkIndex);
        associatedData = associatedData[sizeof(int)..];
        
        MemoryMarshal.Write(associatedData, isTerminal);
    }

    public void Dispose()
    {
        _aesGcm.Dispose();
    }
}
