using AesEncryptionBenchmarks;
using BenchmarkDotNet.Running;

// var summary = BenchmarkRunner.Run<CompareEncryption>();

new CompareEncryption().GlobalSetup();


while (true)
{
    await Task.Delay(200);
}