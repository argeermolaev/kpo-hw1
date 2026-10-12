using Domain.Services;

namespace Domain.Entities.Things
{
    public class ChargingStation : IThing, IEnergyConsumer
    {
        public string InventoryId { get; }
        public double DailyEnergyConsumption { get; }

        public ChargingStation(string inventoryId, double dailyEnergyConsumption)
        {
            if (string.IsNullOrWhiteSpace(inventoryId))
            {
                throw new ArgumentException("Empty inventory id");
            }
            InventoryId = inventoryId;
            DailyEnergyConsumption = dailyEnergyConsumption;
        }
    }
}