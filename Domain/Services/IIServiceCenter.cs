namespace Domain.Services
{
    public interface IServiceCenter
    {
        bool Inspect(ITransport transport);
        bool Inspect(IThing thing);
    }
}