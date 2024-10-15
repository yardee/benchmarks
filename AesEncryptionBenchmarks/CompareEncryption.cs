#nullable enable

using System.Security.Cryptography;
using System.Text;
using AesEncryptionBenchmarks.Encryption;
using BenchmarkDotNet.Attributes;

namespace AesEncryptionBenchmarks;

[MemoryDiagnoser]
public class CompareEncryption
{
    private byte[] _data = [];
    private StreamingEncryptionAesGcm _streamingEncryptionAesGcm = default!;
    
    [GlobalSetup]
    public void GlobalSetup()
    {
        var hugeString = CreateHugeString(990);
        _data = Encoding.UTF8.GetBytes(hugeString);
        _streamingEncryptionAesGcm = new StreamingEncryptionAesGcm(GetEncryptionKey());
    }
    
    [Benchmark]
    public async Task Encrypt()
    {
        using var inputStream = new MemoryStream(_data);
        using var outputStream = new MemoryStream();
        
        await _streamingEncryptionAesGcm.EncryptAsync(inputStream, outputStream, CancellationToken.None);
    }

    
    string CreateHugeString(int sizeInkB)
    {
        int length = sizeInkB * 1024 * 2;
        return new string('A', length);
    }
    
    static byte[] GetEncryptionKey()
    {
        var encryptionKey = new byte[32];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(encryptionKey);
        return encryptionKey;
    }
}