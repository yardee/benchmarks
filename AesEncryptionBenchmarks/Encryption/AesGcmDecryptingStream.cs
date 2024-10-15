using System.Buffers;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace AesEncryptionBenchmarks.Encryption;

public class AesGcmDecryptingStream(Stream encryptedStream, ReadOnlySpan<byte> key) : Stream
{
    readonly AesGcm _aesGcm = new(key, AesGcmConstants.TagSize);
    byte[]? _inputBuffer;
    byte[]? _outputBuffer;
    int _chunkSize;
    int _chunkIndex;
    int _decryptedBytesCount;
    int _outputBufferPosition;
    bool _isInitialized;
    bool EndOfStreamReached => encryptedStream.Length == encryptedStream.Position;
    bool EndOfFileReached => _decryptedBytesCount == 0 && EndOfStreamReached;
    
    public override int Read(byte[] buffer, int offset, int count)
    {
        return ReadAsync(buffer, offset, count).GetAwaiter().GetResult();
    }

    public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        if (!_isInitialized)
        {
            await InitializeAsync(cancellationToken);
        }

        int totalBytesRead = 0;

        try
        {
            while (count > 0 && !EndOfFileReached)
            {
                if (_decryptedBytesCount == 0)
                {
                    await DecryptNextChunkAsync(cancellationToken);
                }

                int bytesRead = Math.Min(count, _decryptedBytesCount);
                Buffer.BlockCopy(_outputBuffer!, _outputBufferPosition, buffer, offset, bytesRead);
                _outputBufferPosition += bytesRead;
                offset += bytesRead;
                count -= bytesRead;
                totalBytesRead += bytesRead;
                _decryptedBytesCount -= bytesRead;
            }
        }
        catch
        {
            ReturnSharedMemory();
            throw;
        }

        return totalBytesRead;
    }

    async Task InitializeAsync(CancellationToken cancellationToken)
    {
        var data = new byte[4];
        await encryptedStream.ReadExactlyAsync(data, cancellationToken);
        if (!data.AsSpan().SequenceEqual(AesGcmConstants.Qev1.Span))
            throw new CryptographicException("Encryption hash mismatch");
        await encryptedStream.ReadExactlyAsync(data, cancellationToken);
        _chunkSize = MemoryMarshal.Read<int>(data);
        if (_chunkSize > AesGcmConstants.MaxChunkSize)
            throw new CryptographicException($"Chunk size is too big: {_chunkSize}");

        _inputBuffer = ArrayPool<byte>.Shared.Rent(_chunkSize);
        _outputBuffer = ArrayPool<byte>.Shared.Rent(_chunkSize - AesGcmConstants.NonceSize - AesGcmConstants.TagSize);
        _isInitialized = true;
    }

    async Task DecryptNextChunkAsync(CancellationToken cancellationToken)
    {
        var inputMemory = _inputBuffer.AsMemory(0, _chunkSize);
        var read = await encryptedStream.ReadAtLeastAsync(inputMemory, _chunkSize, false, cancellationToken);
        var actuallyRead = inputMemory[..read];
        var outputMemory = _outputBuffer.AsMemory(0, read - AesGcmConstants.NonceSize - AesGcmConstants.TagSize);
        DecodeChunk(actuallyRead.Span, outputMemory.Span);
    }
    
    void DecodeChunk(Span<byte> actuallyRead, Span<byte> outputSpan)
    {
        // QEv1 + chunkIndex + TerminalFlag
        // 4    + 4          + 1           = 9
        Span<byte> associatedData = stackalloc byte[9];
        FillAssociatedData(associatedData);
        
        var dataLength = actuallyRead.Length - AesGcmConstants.NonceSize - AesGcmConstants.TagSize;
        _aesGcm.Decrypt(
            actuallyRead[..AesGcmConstants.NonceSize],
            actuallyRead.Slice(AesGcmConstants.NonceSize, dataLength),
            actuallyRead.Slice(actuallyRead.Length - AesGcmConstants.TagSize, AesGcmConstants.TagSize),
            outputSpan,
            associatedData);
        
        _outputBufferPosition = 0;   
        _decryptedBytesCount += dataLength;
    }

    void FillAssociatedData(Span<byte> associatedData)
    {
        AesGcmConstants.Qev1.Span.CopyTo(associatedData);
        associatedData = associatedData[AesGcmConstants.Qev1.Length..];
        
        MemoryMarshal.Write(associatedData, _chunkIndex++);
        associatedData = associatedData[sizeof(int)..];
        
        MemoryMarshal.Write(associatedData, EndOfStreamReached);
    }
    
    void ReturnSharedMemory()
    {
        if (_inputBuffer is not null)
        {
            ArrayPool<byte>.Shared.Return(_inputBuffer);
            _inputBuffer = null;
        }

        if (_outputBuffer is not null)
        {
            ArrayPool<byte>.Shared.Return(_outputBuffer);
            _outputBuffer = null;
        }
    }
    
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            ReturnSharedMemory();
            _aesGcm.Dispose();
            encryptedStream.Dispose();
        }

        base.Dispose(disposing);
    }

    public override bool CanRead => true;
    public override bool CanSeek => false;
    public override bool CanWrite => false;
    public override long Length => throw new NotImplementedException();
    public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public override void Flush() { throw new NotImplementedException(); }
    public override long Seek(long offset, SeekOrigin origin) { throw new NotImplementedException(); }
    public override void SetLength(long value) { throw new NotImplementedException(); }
    public override void Write(byte[] buffer, int offset, int count) { throw new NotImplementedException(); }
}
