using CashRegisterCore.Exceptions;
using CashRegisterCore.Models;
using CashRegisterCore.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Text;

namespace CashRegisterCoreTests
{
    public class ConfigurationInformationServiceTests
    {
        private readonly IConfigurationInformationService _service;
        private readonly string JSON = "{\r\n  \"countries\": [\r\n    {\r\n      \"name\": \"USA\",\r\n      \"billsAndCoins\": [ 0.01, 0.05, 0.10, 0.25, 0.50, 1.00, 2.00, 5.00, 10.00, 20.00, 50.00, 100.00 ]\r\n    },\r\n    {\r\n      \"name\": \"MEXICO\",\r\n      \"billsAndCoins\": [ 0.05, 0.10, 0.20, 0.50, 1.00, 2.00, 5.00, 10.00, 20.00, 50.00, 100.00 ]\r\n    }\r\n  ],\r\n  \"messages\": {\r\n    \"countriesNotFound\": \"Countries not found, please contact your Administrator to configure our Cash Register\",\r\n    \"selectCountry\": \"We need to know what's your country, here we have our current options(enter the number who corresponds to you country and then press Enter):\",\r\n    \"wrongAmountToPay\": \"The introduced amount is not valid, would be grater than 0 and numeric, please introduce the amount with two decimals, for example: 20.00\",\r\n    \"invalidChoice\": \"Your choice is not available, please verify your selection and remember, yo need to select your country by number, write the number and hit enter\",\r\n    \"introduceAmountToPay\": \"For pay please introduce the amount you will pay, it would be entered with two decimals, for example 20.00\",\r\n    \"introduceBillsToPay\": \"Now please introduce the bills/coins with which you will make the payment (please introduce your bills/coins keeping in mind the available bills/coins and separate them by \\\",\\\"), \",\r\n    \"wrongBillsIntroduced\": \"The bills entered are wrong or are not enough to pay the amount you indicated, please enter valid bills\",\r\n\r\n\r\n    \"successfulPayment\": \"Your payment has been successful, do you want to make another payment?... in that case please enter \\\"yes\\\", in other case you can exit from our Cash Register\",\r\n    \"goodBye\": \"Thanks for use our Cash Register, come again soon!\"\r\n  }\r\n}";
        public ConfigurationInformationServiceTests()
        {

            var configuration = new ConfigurationBuilder()
            .AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(JSON)))
            .Build();

            _service = new ConfigurationInformationService(configuration);
        }
        [Fact(DisplayName = "When Program Try To Retrieve Messages Then They Are Returned")]
        public void WhenProgramTryToRetrieveMessagesThenTheyAreReturned()
        {
            var result = _service.GetMessages();

            Assert.NotEmpty(result);
        }

        [Fact(DisplayName = "When Program Try To Retrieve Messages And No Messages Then They Are Returned")]
        public void WhenProgramTryToRetrieveMessagesAndNoMessagesThenTheyAreReturned()
        {
            IConfiguration config = new ConfigurationBuilder()
            .AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes("{}")))
            .Build();
            var service = new ConfigurationInformationService(config);

            Assert.Throws<ConfigurationException>(() => service.GetMessages());
        }

        [Fact(DisplayName = "When Program Try To Retrieve Countries Then They Are Returned")]
        public void WhenProgramTryToRetrieveCountriesThenTheyAreReturned()
        {
            var result = _service.GetCountries();

            Assert.NotEmpty(result);
        }

        [Fact(DisplayName = "When Program Try To Retrieve Countries And No Countries Then They Are Returned")]
        public void WhenProgramTryToRetrieveCountriesAndNoCountriesThenTheyAreReturned()
        {
            IConfiguration config = new ConfigurationBuilder()
            .AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes("{}")))
            .Build();
            var service = new ConfigurationInformationService(config);

            Assert.Throws<ConfigurationException>(() => service.GetCountries());
        }

    }
}