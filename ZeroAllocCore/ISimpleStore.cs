namespace ZeroAllocCore;

public interface ISimpleStore
{
    void Set(string? key, UserProfile? profile);
    UserProfile? Get(string? key);
    bool Remove(string? key);
    (long setCount, long getCount, long deleteCount) GetStatistics();
}