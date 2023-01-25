using CashRegisterCore.Models;
using CashRegisterCore.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Text;

namespace CashRegisterCoreTests
{
    public class UserInputServiceTests
    {
        private readonly IUserInputService _service;
        private readonly string JSON = "{\r\n  \"countries\": [\r\n    {\r\n      \"name\": \"USA\",\r\n      \"billsAndCoins\": [ 0.01, 0.05, 0.10, 0.25, 0.50, 1.00, 2.00, 5.00, 10.00, 20.00, 50.00, 100.00 ]\r\n    },\r\n    {\r\n      \"name\": \"MEXICO\",\r\n      \"billsAndCoins\": [ 0.05, 0.10, 0.20, 0.50, 1.00, 2.00, 5.00, 10.00, 20.00, 50.00, 100.00 ]\r\n    }\r\n  ],\r\n  \"messages\": {\r\n    \"countriesNotFound\": \"Countries not found, please contact your Administrator to configure our Cash Register\",\r\n    \"selectCountry\": \"We need to know what's your country, here we have our current options(enter the number who corresponds to you country and then press Enter):\",\r\n    \"wrongAmountToPay\": \"The introduced amount is not valid, would be grater than 0 and numeric, please introduce the amount with two decimals, for example: 20.00\",\r\n    \"invalidChoice\": \"Your choice is not available, please verify your selection and remember, yo need to select your country by number, write the number and hit enter\",\r\n    \"introduceAmountToPay\": \"For pay please introduce the amount you will pay, it would be entered with two decimals, for example 20.00\",\r\n    \"introduceBillsToPay\": \"Now please introduce the bills/coins with which you will make the payment (please introduce your bills/coins keeping in mind the available bills/coins and separate them by \\\",\\\"), \",\r\n    \"wrongBillsIntroduced\": \"The bills entered are wrong or are not enough to pay the amount you indicated, please enter valid bills\",\r\n\r\n\r\n    \"successfulPayment\": \"Your payment has been successful, do you want to make another payment?... in that case please enter \\\"yes\\\", in other case you can exit from our Cash Register\",\r\n    \"goodBye\": \"Thanks for use our Cash Register, come again soon!\"\r\n  }\r\n}";
        public UserInputServiceTests()
        {

            var configuration = new ConfigurationBuilder()
            .AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(JSON)))
            .Build();

            _service = new UserInputService(configuration);
        }
        [Fact(DisplayName = "When User Enters a right amount then service returns the amount provided")]
        public void WheUserEntersRightAmountThenTheAmountIsRetrieved()
        {
            Console.SetIn(new StringReader("66.66"));
            var result = _service.GetAmountToPay();

            Assert.Equal(66.66f, result);
        }

        [Fact(DisplayName = "When User Enters a wrong amount then service asks for correction")]
        public void WheUserEntersWrongAmountThenAppAsksForCorrection()
        {
            string inputString = "fifty\n50";
            Console.SetIn(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(inputString))));
            var result = _service.GetAmountToPay();

            Assert.Equal(50f, result);
        }

        [Fact(DisplayName = "Whe User Select A Valid Country Then Selected Country IsReturned")]
        public void WheUserSelectAValidCountryThenSelectedCountryIsReturned()
        {
            Console.SetIn(new StringReader("1"));
            List<Country> countries = GetCountries();
            var result = _service.SelectCountry(countries);

            Assert.Equal(1, result);
        }

        [Fact(DisplayName = "Whe User Select A wrong Country Then service asks for correction")]
        public void WheUserSelectAWrongCountryThenServiceAsksForCorrection()
        {
            string inputString = "a\n2";
            Console.SetIn(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(inputString))));
            List<Country> countries = GetCountries();
            var result = _service.SelectCountry(countries);

            Assert.Equal(2, result);
        }

        [Fact(DisplayName = "Whe User Provides Right Bills Then Service Return The Bills")]
        public void WheUserProvidesRightBillsThenServiceReturnTheBills()
        {
            string inputString = "20.00,20.00,20.00";
            Console.SetIn(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(inputString))));
            List<Country> countries = GetCountries();
            var result = _service.GetBillsFromUser(countries, 2, 50.00f);

            Assert.Equal(3, result.Length);
        }

        [Fact(DisplayName = "When User Provides Wrong Bill Then Service Asks For Change And The Bills")]
        public void WhenUserProvidesWrongBillThenServiceAsksForChangeAndTheBills()
        {
            string inputString = "a\n20.00,20.00,20.00";
            Console.SetIn(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(inputString))));
            List<Country> countries = GetCountries();
            var result = _service.GetBillsFromUser(countries, 2, 50.00f);

            Assert.Equal(3, result.Length);
        }

        [Fact(DisplayName = "When User Provides Invalid Bill Then Service Asks For Change And The Bills")]
        public void WhenUserProvidesInvalidBillThenServiceAsksForChangeAndTheBills()
        {
            string inputString = "55\n20.00,20.00,20.00";
            Console.SetIn(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(inputString))));
            List<Country> countries = GetCountries();
            var result = _service.GetBillsFromUser(countries, 2, 50.00f);

            Assert.Equal(3, result.Length);
        }

        [Fact(DisplayName = "When User Doesnt Provide Enough Money Then Service Asks For Change And The Bills")]
        public void WhenUserDoesntProvideEnoughMoneyThenServiceAsksForChangeAndTheBills()
        {
            string inputString = "20.00,20.00\n20.00,20.00,20.00";
            Console.SetIn(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(inputString))));
            List<Country> countries = GetCountries();
            var result = _service.GetBillsFromUser(countries, 2, 50.00f);

            Assert.Equal(3, result.Length);
        }

        [Fact(DisplayName = "When User Doesnt Provide Anything Then Service Asks For Change And The Bills")]
        public void WhenUserDoesntProvideAnythingThenServiceAsksForChangeAndTheBills()
        {
            string inputString = "\n20.00,20.00,20.00";
            Console.SetIn(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(inputString))));
            List<Country> countries = GetCountries();
            var result = _service.GetBillsFromUser(countries, 2, 50.00f);

            Assert.Equal(3, result.Length);
        }

        private static List<Country> GetCountries()
        {
            return new()
            {
                new Country()
                {
                    Name = "USA",
                    BillsAndCoins = new float[]{ 0.01f, 0.05f, 0.10f, 0.25f, 0.50f, 1.00f, 2.00f, 5.00f, 10.00f, 20.00f, 50.00f, 100.00f }
                },
                new Country()
                {
                    Name = "MEXICO",
                    BillsAndCoins = new float[]{ 0.05f, 0.10f, 0.20f, 0.50f, 1.00f, 2.00f, 5.00f, 10.00f, 20.00f, 50.00f, 100.00f }
                }
            };
        }
    }
}