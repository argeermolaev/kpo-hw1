using Domain.Services;

namespace Application
{
    public class ParkService
    {
        private readonly IServiceCenter _serviceCenter;
        private readonly List<ITransport> _transports = new();
        private readonly List<IThing> _things = new();

        public ParkService(IServiceCenter serviceCenter)
        {
            _serviceCenter = serviceCenter;
        }

        public bool AddTransport(ITransport transport)
        {
            if (_serviceCenter.Inspect(transport))
            {
                _transports.Add(transport);
                return true;
            }
            return false;
        }

        public bool AddThing(IThing thing)
        {
            if (_serviceCenter.Inspect(thing))
            {
                _things.Add(thing);
                return true;
            }
            return false;
        }

        public double GetTotalDailyEnergy()
        {
            double transportEnergy = _transports.OfType<IEnergyConsumer>().Sum(t => t.DailyEnergyConsumption);
            double thingsEnergy = _things.OfType<IEnergyConsumer>().Sum(t => t.DailyEnergyConsumption);

            return transportEnergy + thingsEnergy;
        }

        public List<ITransport> GetTransportsForNovices()
        {
            return _transports.Where(t => t.NoviceFriendliness >= 6).ToList();
        }

        public List<string> GetAllInventoryIds()
        {
            var transportIds = _transports.Select(t => t.InventoryId);
            var thingIds = _things.Select(t => t.InventoryId);

            return transportIds.Concat(thingIds).ToList();
        }

        public List<string> GetTransportIds()
        {
            var transportIds = _transports.Select(t => t.InventoryId);

            return transportIds.ToList();
        }

        public List<string> GetThingIds()
        {
            var thingIds = _things.Select(t => t.InventoryId);

            return thingIds.ToList();
        }
    }
}
