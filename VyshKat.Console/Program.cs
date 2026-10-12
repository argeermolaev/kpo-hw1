using Microsoft.Extensions.DependencyInjection;
using Application;
using Application.Services;
using Domain.Services;
using System;

namespace VyshKatConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IServiceCenter, ServiceCenter>()
                .AddSingleton<ParkService>()
                .BuildServiceProvider();

            var parkService = serviceProvider.GetRequiredService<ParkService>();

            var menu = new ConsoleMenu(parkService);
            menu.Run();
        }
    }
}