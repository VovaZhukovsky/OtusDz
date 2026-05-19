using ZeroAllocCore;

namespace ZeroAllocCoreTest;

public class CommandParserTests
{
    [Fact]
    public void Validate_Set_Command_With_Three_Args_Should_Be_Valid()
    {
        var result = CommandParser.Parse("set user:1 data");
        Assert.True(result.Command is "set");
        Assert.True(result.Key is "user:1");
        Assert.True(result.Value is "data");
    }

    [Fact]
    public void Validate_Get_Command_With_Two_Args_Should_Be_Valid()
    {
        var result = CommandParser.Parse("get user:1");
        Assert.True(result.Command is "get");
        Assert.True(result.Key is "user:1");
        Assert.True(result.Value.IsEmpty);
    }

    [Fact]
    public void Validate_Incorrect_Command_Should_Be_Empty_Result()
    {
        var result = CommandParser.Parse("invalidCommand");
        Assert.True(result.Command.IsEmpty);
        Assert.True(result.Key.IsEmpty);
        Assert.True(result.Value.IsEmpty);
    }

    [Fact]
    public void Validate_Command_With_Extra_Spaces_Should_Be_Valid()
    {
        var result = CommandParser.Parse("set  user:1  data");
        Assert.True(result.Command is "set");
        Assert.True(result.Key is "user:1");
        Assert.True(result.Value is "data");
    }
}