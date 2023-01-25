using CashRegisterCore.Models;
using CashRegisterCore.Services;
using CashRegisterCore.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    private static void Main(string[] args)
    {
        IConfiguration configuration = GetConfiguration();
        ServiceProvider serviceProvider = ConfigureDependencyInyection(configuration);

        var configService = serviceProvider.GetRequiredService<IConfigurationInformationService>();
        var userInputService = serviceProvider.GetRequiredService<IUserInputService>();
        var cashRegisterService = serviceProvider.GetRequiredService<ICashRegisterService>();
        try
        {
            Dictionary<string, string?> messages = configService.GetMessages();
            List<Country> countries = configService.GetCountries();

            Console.WriteLine("Hello and welcome to our Cash Register, please follow our instructions");        

            bool makeAnotherPayment = true;

            while (makeAnotherPayment)
            {
                int selectedCountry = userInputService.SelectCountry(countries);
                float amountToPay = userInputService.GetAmountToPay();
                float[] bills = userInputService.GetBillsFromUser(countries, selectedCountry, amountToPay);
               
                Console.WriteLine($"You will pay: {amountToPay} with the following bills/coins: {string.Join(", ", bills.Select(f => f.ToString("0.00")))}");
                List<float> changeBills = cashRegisterService.GetMinimumBillsChange(amountToPay, bills, countries[selectedCountry - 1].BillsAndCoins);
            
                Console.WriteLine("Your payment has been successfull, here is your change:\n"
                    + $"{string.Join(",", changeBills.Select(f=>f.ToString("0.00")))}\n"
                    + "Do you want to make another payment?... in that case please enter \"yes\", in other case you can exit from our Cash Register");
                string? MakeAnotherOperation = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(MakeAnotherOperation) || !MakeAnotherOperation.Contains("yes", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(messages.GetValueOrDefault("goodBye"));
                    Environment.Exit(0);
                }
            }
        }
        catch (ConfigurationException e)
        {
            Console.WriteLine(e.Message);
            Environment.Exit(0);
        }
    }
    // Method to configure dependency inyection
    private static ServiceProvider ConfigureDependencyInyection(IConfiguration configuration)
    {
        return new ServiceCollection()
                    .AddSingleton<IConfigurationInformationService, ConfigurationInformationService>()
                    .AddSingleton<IUserInputService, UserInputService>()
                    .AddSingleton<ICashRegisterService, CashRegisterService>()
                    .AddSingleton(configuration)
                    .BuildServiceProvider();
    }

    // Method to build and retrieve configuration
    private static IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                         .Build();
    }
}