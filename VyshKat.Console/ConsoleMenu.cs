using Application;
using Domain.Entities.Things;
using Domain.Entities.Transport;
using Domain.Services;

namespace VyshKatConsole
{
    public class ConsoleMenu
    {
        private readonly ParkService _parkService;

        public ConsoleMenu(ParkService parkService)
        {
            _parkService = parkService;
        }

        public void Run()
        {
            // Предзаполним парк данными для демонстрации (Seed Data)
            SeedData();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Система учета парка «Вышкат» ===");
                Console.WriteLine("1. Добавить транспорт (с техосмотром)");
                Console.WriteLine("2. Добавить вещь (с проверкой)");
                Console.WriteLine("3. Показать суммарное суточное потребление энергии");
                Console.WriteLine("4. Показать транспорт для новичков");
                Console.WriteLine("5. Показать инвентарные номера всей техники и вещей");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddTransport(); break;
                    case "2": AddThing(); break;
                    case "3": ShowTotalEnergy(); break;
                    case "4": ShowNoviceTransports(); break;
                    case "5": ShowInventoryReport(); break;
                    case "0": return;
                    default: Console.WriteLine("Неверный ввод. Нажмите любую клавишу..."); Console.ReadKey(); break;
                }
            }
        }

        // --- Реализация сценариев ---

        private void AddTransport()
        {
            Console.WriteLine("\nВыберите тип транспорта:");
            Console.WriteLine("1. Электросамокат");
            Console.WriteLine("2. Велосипед");
            Console.WriteLine("3. Электровелосипед");
            Console.WriteLine("4. Самокат");
            Console.Write("Тип: ");
            var type = Console.ReadLine();

            Console.Write("Введите инвентарный номер: ");
            var id = Console.ReadLine();

            Console.Write("Введите простоту для новичка (1-10): ");
            if (!int.TryParse(Console.ReadLine(), out int friendliness))
            {
                Console.WriteLine("Ошибка: нужно число.");
                Console.ReadKey();
                return;
            }

            double energyconsumption = 0.0;

            if (type == "1" || type == "3")
            {
                Console.Write("Введите энергопотребление: ");
                if (!double.TryParse(Console.ReadLine(), out energyconsumption))
                {
                    Console.WriteLine("Ошибка: нужно число.");
                    Console.ReadKey();
                    return;
                }
            }

            ITransport transport = type switch
            {
                "1" => new ElectricScooter(id, friendliness, energyconsumption),
                "2" => new Bicycle(id, friendliness),
                "3" => new ElectricBicycle(id, friendliness, energyconsumption),
                "4" => new Scooter(id, friendliness),
                _ => null
            };

            if (transport == null) {
                Console.WriteLine("Неверный тип."); 
                Console.ReadKey(); 
                return;
            }

            bool success = _parkService.AddTransport(transport);

            if (success)
                Console.WriteLine("Транспорт успешно прошел техосмотр и добавлен в парк!");
            else
                Console.WriteLine("Транспорт НЕ прошел техосмотр и был отклонен.");

            Console.ReadKey();
        }

        private void AddThing()
        {
            Console.WriteLine("\nВыберите тип вещи:");
            Console.WriteLine("1. Шлем");
            Console.WriteLine("2. Зарядная станция");
            Console.Write("Тип: ");
            var type = Console.ReadLine();

            Console.Write("Введите инвентарный номер: ");
            var id = Console.ReadLine();

            double energyconsumption = 0.0;

            if (type == "2")
            {
                Console.Write("Введите энергопотребление: ");
                if (!double.TryParse(Console.ReadLine(), out energyconsumption))
                {
                    Console.WriteLine("Ошибка: нужно число.");
                    Console.ReadKey();
                    return;
                }
            }

            IThing thing = type switch
            {
                "1" => new Helmet(id),
                "2" => new ChargingStation(id, energyconsumption),
                _ => null
            };

            if (thing == null)
            {
                Console.WriteLine("Неверный тип.");
                Console.ReadKey();
                return;
            }

            bool success = _parkService.AddThing(thing);

            if (success)
                Console.WriteLine("Вещь успешно прошла проверку и добавлена в парк!");
            else
                Console.WriteLine("Вещь НЕ прошла проверку и была отклонена.");

            Console.ReadKey();
        }

        private void ShowTotalEnergy()
        {
            double energy = _parkService.GetTotalDailyEnergy();
            Console.WriteLine($"\nСуммарное суточное потребление энергии: {energy} кВт*ч");
            Console.ReadKey();
        }

        private void ShowNoviceTransports()
        {
            var transports = _parkService.GetTransportsForNovices();
            Console.WriteLine("\nТранспорт, подходящий для новичков:");
            if (!transports.Any()) Console.WriteLine("  (нет подходящего транспорта)");

            foreach (var t in transports)
            {
                Console.WriteLine($"  - {t.GetType().Name} (ID: {t.InventoryId}, Простота: {t.NoviceFriendliness})");
            }
            Console.ReadKey();
        }

        private void ShowInventoryReport()
        {
            Console.WriteLine($"Список всех предметов: {string.Join(", ", _parkService.GetAllInventoryIds())}");
            Console.WriteLine("\nИнвентарные номера техники и вещей:");
            Console.WriteLine($"  - Транспорт: {string.Join(", ", _parkService.GetTransportIds())}");
            Console.WriteLine($"  - Вещи: {string.Join(", ", _parkService.GetThingIds())}");
            Console.ReadKey();
        }

        private void SeedData()
        {
            _parkService.AddTransport(new ElectricScooter("SC-001", 8, 1.2));
            _parkService.AddTransport(new Bicycle("BK-002", 4));
            _parkService.AddTransport(new ElectricScooter("SC-003", 2, -5));

            _parkService.AddThing(new Helmet("HL-001"));
            _parkService.AddThing(new ChargingStation("CS-001", 2.5));
        }
    }
}