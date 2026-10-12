using Domain.Services;

namespace Domain.Entities.Things
{
    public class Helmet : IThing
    {
        public string InventoryId { get; }

        public Helmet(string inventoryId)
        {
            if (string.IsNullOrWhiteSpace(inventoryId))
            {
                throw new ArgumentException("Empty inventory id");
            }
            InventoryId = inventoryId;
        }
    }
}