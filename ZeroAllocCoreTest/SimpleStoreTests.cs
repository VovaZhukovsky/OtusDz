using ZeroAllocCore;

namespace ZeroAllocCoreTest;

public class SimpleStoreTests
{
    [Fact]
    public async Task Validate_Parallel_Commands_Should_Be_Valid()
    {
        var store = new SimpleStore();
        var tasks = new List<Task>();
        tasks.Add(Task.Run(() => store.Set("test0", [1])));
        tasks.Add(Task.Run(() => store.Get("test0")));
        tasks.Add(Task.Run(() => store.Set("test1", [2])));
        tasks.Add(Task.Run(() => store.Get("test1")));
        tasks.Add(Task.Run(() => store.Set("test2", [3])));
        tasks.Add(Task.Run(() => store.Set("test2", [4])));
        tasks.Add(Task.Run(() => store.Get("test2")));
        await Task.WhenAll(tasks);
        Assert.Equal(store.Get("test0"), [1]);
        Assert.Equal(store.Get("test1"), [2]);
        var value = store.Get("test2");
        Assert.True(value.SequenceEqual(new byte[]{3}) || value.SequenceEqual(new byte[]{4}) );
        
        Assert.Equal(store.GetStatistics(), (4,6,0));
    }
}