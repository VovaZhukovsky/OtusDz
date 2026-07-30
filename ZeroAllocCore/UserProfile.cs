namespace ZeroAllocCore;

[GenerateBinarySerializer]
public partial class UserProfile
{
    private static int _nextId;
    public int Id { get; set; } = Interlocked.Increment(ref _nextId);
    public string? Name { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    
}