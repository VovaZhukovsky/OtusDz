namespace ZeroAllocCore;

public interface ISimpleStore
{
    void Set(string? key, byte[]? value);
    byte[]? Get(string? key);
    bool Remove(string? key);
}