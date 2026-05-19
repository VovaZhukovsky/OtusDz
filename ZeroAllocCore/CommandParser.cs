namespace ZeroAllocCore;

public static class CommandParser
{
    public static CommandParserResponse Parse(ReadOnlySpan<char> input)
    {
        int index0 = input.IndexOf(' ');
        
        if (index0 == -1)
            return new CommandParserResponse();
        
        ReadOnlySpan<char> command = input.Slice(0, index0);
        input = input.Slice(index0 + 1).TrimStart();
        int index1 =  input.IndexOf(' ');
        
        if (index1 == -1)
            return new CommandParserResponse(command, input);
        
        ReadOnlySpan<char> key = input.Slice(0, index1);
        ReadOnlySpan<char> value = input.Slice(index1 + 1).TrimStart();
        return new CommandParserResponse(command, key, value);
    }
}
public readonly ref struct CommandParserResponse
{
    public readonly ReadOnlySpan<char> Command { get; } = ReadOnlySpan<char>.Empty;
    public readonly ReadOnlySpan<char> Key { get; } = ReadOnlySpan<char>.Empty;
    public readonly ReadOnlySpan<char> Value { get; } = ReadOnlySpan<char>.Empty;
    
    public CommandParserResponse(
        ReadOnlySpan<char> command,
        ReadOnlySpan<char> key,
        ReadOnlySpan<char> value = default)
    {
        Command = command;
        Key = key;
        Value = value;
    }

}
