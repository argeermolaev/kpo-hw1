using Domain.Services;

namespace Domain.Entities.Transport
{
    public class Bicycle : ITransport
    {
        public string InventoryId { get; }
        public int NoviceFriendliness { get; }
        public Bicycle(string inventoryId, int noviceFriendliness)
        {
            if (string.IsNullOrWhiteSpace(inventoryId))
            {
                throw new ArgumentException("Empty inventory id");
            }
            InventoryId = inventoryId;
            if (noviceFriendliness < 1 || noviceFriendliness > 10)
            {
                throw new ArgumentException("Novice friendliness out of range");
            }
            NoviceFriendliness = noviceFriendliness;
        }
    }
}