using System.Text.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using ZeroAllocCore;

namespace SourceGeneratorTest;

[MemoryDiagnoser]
public class SourceGeneratorTest
{
    [Benchmark]
    public void TestSourceGenerator()
    {
        var userProfile = new UserProfile()
        {
            Id = 1,
            Name = "User1",
            Created = DateTime.Now,
        };
        
        var bytes = userProfile.SerializeToBinary();
        UserProfile.DeserializeFromBinary(bytes);
    }

    [Benchmark]
    public void TestSystemTextJson()
    {
        var userProfile = new UserProfile()
        {
            Id = 1,
            Name = "User1",
            Created = DateTime.Now,
        };
        var bytes = JsonSerializer.SerializeToUtf8Bytes(userProfile);
        JsonSerializer.Deserialize<UserProfile>(bytes);
        
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<SourceGeneratorTest>(new DebugBuildConfig());
    }
}