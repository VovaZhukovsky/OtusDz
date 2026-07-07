using ZeroAllocCore;

namespace ZeroAllocCoreTest;

public class SimpleStoreTests
{
    [Fact]
    public async Task Validate_Parallel_Commands_Should_Be_Valid()
    {
        var store = new SimpleStore();
        var tasks = new List<Task>();
        tasks.Add(Task.Run(() => store.Set("test0", new UserProfile(){Name = "test0", Id = 1})));
        tasks.Add(Task.Run(() => store.Get("test0")));
        tasks.Add(Task.Run(() => store.Set("test1", new UserProfile(){Name = "test1", Id = 2})));
        tasks.Add(Task.Run(() => store.Get("test1")));
        tasks.Add(Task.Run(() => store.Set("test2", new UserProfile(){Name = "test2", Id = 3})));
        tasks.Add(Task.Run(() => store.Set("test2", new UserProfile(){Name = "test2", Id = 4})));
        tasks.Add(Task.Run(() => store.Set("test5", new UserProfile(){})));
        tasks.Add(Task.Run(() => store.Set("test6",null)));
        tasks.Add(Task.Run(() => store.Get("test2")));
        await Task.WhenAll(tasks);
        Assert.True(tasks.All(task => task.IsCompletedSuccessfully));
        
        Assert.Equal(store.GetStatistics(), (6,3,0));
    }
}