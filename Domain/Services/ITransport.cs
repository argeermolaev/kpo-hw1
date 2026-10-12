namespace Domain.Services
{
    public interface ITransport : IInventoryItem
    {
        int NoviceFriendliness { get; } // 1-10
    }
}