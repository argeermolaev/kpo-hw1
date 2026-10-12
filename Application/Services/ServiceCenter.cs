using Domain.Services;

namespace Application.Services
{
    public class ServiceCenter : IServiceCenter
    {
        public bool Inspect(ITransport transport)
        {

            if (transport is IEnergyConsumer consumer && consumer.DailyEnergyConsumption <= 0)
            {
                return false;
            }

            return true;
        }

        public bool Inspect(IThing thing)
        {
            if (string.IsNullOrWhiteSpace(thing.InventoryId))
            {
                return false;
            }

            return true;
        }
    }
}