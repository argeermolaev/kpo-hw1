using Domain.Services;

namespace Domain.Entities.Transport
{
    public class ElectricScooter : ITransport, IEnergyConsumer
    {
        public string InventoryId { get; }
        public int NoviceFriendliness { get; }
        public double DailyEnergyConsumption { get; }
        public ElectricScooter(string inventoryId, int noviceFriendliness, double dailyEnergyConsumption)
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
            DailyEnergyConsumption = dailyEnergyConsumption;
        }
    }
}
