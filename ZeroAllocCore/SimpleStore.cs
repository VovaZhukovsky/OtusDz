namespace ZeroAllocCore;

public class SimpleStore : ISimpleStore
{
    private readonly Dictionary<string, byte[]> _store = new();
    public void Set(string key, byte[] value)
    {
        _store[key] = value;
    }

    public byte[]? Get(string key)
    {
        return _store.GetValueOrDefault(key);
    }

    public bool Remove(string key)
    {
        return _store.Remove(key);
    }
}