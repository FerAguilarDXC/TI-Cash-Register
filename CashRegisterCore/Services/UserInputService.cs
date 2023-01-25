using CashRegisterCore.Exceptions;
using CashRegisterCore.Models;
using Microsoft.Extensions.Configuration;

namespace CashRegisterCore.Services
{
    public class UserInputService : IUserInputService
    {
        private readonly Dictionary<string, string?> _messages;
        public UserInputService(IConfiguration configuration)
        {
            _messages = configuration.GetSection("messages")
                                      .GetChildren()
                                      .ToDictionary(x => x.Key, x => x.Value);
        }
        /// <summary>
        /// This method retrieve the selected country by user, the service asks for a valid country
        /// </summary>
        /// <param name="countries">The <see cref="List<Country>"/> Available countries to select country</param>
        /// <returns>The <see cref="int"/> For selected country.</returns>
        public int SelectCountry(List<Country> countries)
        {
            Console.WriteLine(_messages.GetValueOrDefault("selectCountry"));

            int selectedCountry;
            for (int i = 0; i < countries.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {countries[i].Name}");
            }
            int.TryParse(Console.ReadLine(), out selectedCountry);
            if (selectedCountry < 1 || selectedCountry > countries.Count)
            {
                while (selectedCountry < 1 || selectedCountry > countries.Count)
                {
                    Console.WriteLine(_messages.GetValueOrDefault("invalidChoice"));
                    int.TryParse(Console.ReadLine(), out selectedCountry);
                }
            }
            return selectedCountry;
        }
        /// <summary>
        /// This method retrieve the amount to pay for user
        /// </summary>
        /// <returns>The <see cref="float"/> amount to pay.</returns>
        public float GetAmountToPay()
        {
            float amountToPay = 0;
            while(amountToPay <= 0)
            {
                Console.WriteLine(_messages.GetValueOrDefault("introduceAmountToPay"));
                float.TryParse(Console.ReadLine(), out amountToPay);
                if(amountToPay <= 0)
                {
                    Console.WriteLine(_messages.GetValueOrDefault("wrongAmountToPay"));
                }
            }
            return amountToPay;
        }
        /// <summary>
        /// This method retrieve the bills entered by user
        /// </summary>
        /// <param name="countries">The <see cref="List<Country>"/> Available countries to select country</param>
        /// <param name="selectedCountry">The <see cref="List<Country>"/> Selected country by user</param>
        /// <param name="amountToPay">The <see cref="List<Country>"/> Amount to pay by user</param>
        /// <returns>The <see cref="float[]"/> List bills provided by user.</returns>
        public float[] GetBillsFromUser(List<Country> countries, int selectedCountry, float amountToPay)
        {
            string billsAvailable = string.Join(", ", countries[selectedCountry - 1].BillsAndCoins.Select(f => f.ToString("0.00")));
            string exampleBillsAvailable = string.Join(", ", countries[selectedCountry - 1].BillsAndCoins.Select(f => f.ToString("0.00")).Take(3));

            Console.WriteLine($"For your country selection({countries[selectedCountry - 1].Name}) we have the following bills/coins: {billsAvailable}");
            float[] bills = Array.Empty<float>();
            bool wrongBills = true;
            while (wrongBills)
            {
                try
                {
                    Console.WriteLine(_messages.GetValueOrDefault("introduceBillsToPay") + $"for example {exampleBillsAvailable}");

                    string? stringBills = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(stringBills))
                    {
                        if (stringBills.IndexOf(",") > 0)
                        {
                            bills = Array.ConvertAll(stringBills.Split(','), float.Parse);
                        }
                        else
                        {
                            bills = new float[] { float.Parse(stringBills) };
                        }
                        ValidateBills(countries, selectedCountry, bills, amountToPay);
                        wrongBills = false;
                    }
                    else
                    {
                        Console.WriteLine(_messages.GetValueOrDefault("wrongBillsIntroduced"));
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine(_messages.GetValueOrDefault("wrongBillsIntroduced"));
                } 
                catch(WrongUserInputException e)
                {
                    Console.WriteLine(_messages.GetValueOrDefault("wrongBillsIntroduced") + e.Message);
                }
            }
            
            return bills;
        }

        private static void ValidateBills(List<Country> countries, int selectedCountry, float[] bills, float amountToPay)
        {
            var wrongBills = new List<string>();
            foreach (var bill in bills)
            {
                if (!countries[selectedCountry - 1].BillsAndCoins.Contains(bill))
                {
                    wrongBills.Add(bill.ToString("0.00"));
                }
            }
            if (wrongBills.Count > 0)
            {
                throw new WrongUserInputException($"\nThe following enterd bills are wrong: {string.Join(", ", wrongBills)}");
            }

            float billsAmount = bills.Sum();
            if (billsAmount <= 0 || amountToPay > billsAmount)
            {
                throw new WrongUserInputException("\nBills not enough to pay");
            }
        }
    }
}
