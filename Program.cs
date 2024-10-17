using System;
using System.Threading;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        Airline airline = new Airline();
        using (CancellationTokenSource cts = new CancellationTokenSource())
        {
            
            cts.CancelAfter(TimeSpan.FromSeconds(15));

            Console.WriteLine("Запуск операций авиакомпании...\n");
            try
            {
                await airline.RunOperationsAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Операции авиакомпании были отменены.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Общая ошибка: {ex.Message}");
            }
        }
        Console.WriteLine("\nВсе операции завершены успешно.");
    }
}

public class TicketBooking
{
    private static readonly Random _random = new Random();

    public async Task ProcessBookingAsync(int numberOfTickets, CancellationToken token)
    {
        for (int i = 1; i <= numberOfTickets; i++)
        {
            Console.WriteLine($"\nНачинается бронирование билета {i}...");
            int delay = _random.Next(1000, 3000); 

            for (int j = 0; j < delay / 100; j++)
            {
                await Task.Delay(100, token);
                Console.Write("."); 
            }

            Console.WriteLine($"\nБронирование билета {i} завершено успешно.");
        }
    }
}

public class CustomerService
{
    private static readonly Random _random = new Random();

    public async Task HandleRequestAsync(CancellationToken token)
    {
        Console.WriteLine("\nЗапрос клиента обрабатывается...");
        int delay = _random.Next(2000, 4000); 

        for (int i = 0; i < delay / 100; i++)
        {
            await Task.Delay(100, token);
            Console.Write("."); 
        }

        Console.WriteLine("\nЗапрос клиента обработан успешно.");
    }
}

public class Maintenance
{
    private static readonly Random _random = new Random();

    public async Task PerformMaintenanceAsync(CancellationToken token)
    {
        Console.WriteLine("\nНачинается техническое обслуживание самолета...");
        int delay = _random.Next(3000, 6000); 

        for (int i = 0; i < delay / 100; i++)
        {
            await Task.Delay(100, token);
            Console.Write("."); 
        }

        Console.WriteLine("\nТехническое обслуживание самолета завершено успешно.");
    }
}

public class Airline
{
    private readonly TicketBooking _ticketBooking = new TicketBooking();
    private readonly CustomerService _customerService = new CustomerService();
    private readonly Maintenance _maintenance = new Maintenance();

    public async Task RunOperationsAsync(CancellationToken cancellationToken)
    {
        try
        {
            Task bookingTask = _ticketBooking.ProcessBookingAsync(5, cancellationToken); 
            Task customerServiceTask = _customerService.HandleRequestAsync(cancellationToken);
            Task maintenanceTask = _maintenance.PerformMaintenanceAsync(cancellationToken);

            await Task.WhenAll(bookingTask, customerServiceTask, maintenanceTask);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Общая ошибка в системе авиакомпании: {ex.Message}");
        }
    }
}
