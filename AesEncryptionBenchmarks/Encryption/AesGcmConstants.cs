namespace AesEncryptionBenchmarks.Encryption;

public static class AesGcmConstants
{
    public const int MaxChunkSize = 512 * 1024;
    public const int NonceSize = 12;
    public const int TagSize = 16;
    public static readonly ReadOnlyMemory<byte> Qev1 = "QEv1"u8.ToArray();
}
