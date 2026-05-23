namespace ZeroAllocCore;

public class SimpleStore : ISimpleStore
{
    private readonly Dictionary<string, byte[]> _store = new();
    public void Set(string? key, byte[]? value)
    {
        if (key is not null && value is not null)
            _store[key] = value;
    }

    public byte[]? Get(string? key)
    {
        if (key is null)
            return null;
        
        return _store.GetValueOrDefault(key);
    }

    public bool Remove(string? key)
    {
        if (key is null)
            return false;
            
        return _store.Remove(key);
    }
}