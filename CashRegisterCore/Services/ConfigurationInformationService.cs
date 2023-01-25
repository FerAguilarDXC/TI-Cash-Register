using CashRegisterCore.Exceptions;
using CashRegisterCore.Models;
using Microsoft.Extensions.Configuration;

namespace CashRegisterCore.Services
{
    public class ConfigurationInformationService : IConfigurationInformationService
    {
        private readonly Dictionary<string, string?> _messages;
        private readonly IConfiguration _configuration;

        // Constructor to Inyect configuration neeed by service 
        public ConfigurationInformationService(IConfiguration configuration)
        {
            _messages = configuration.GetSection("messages")
                                      .GetChildren()
                                      .ToDictionary(x => x.Key, x => x.Value);
            _configuration = configuration;
        }
        /// <summary>
        /// Method to retrieve messages needed for application from configuration
        /// </summary>
        /// <returns>The <see cref="Dictionary<string, string?>"/>.</returns>
        public Dictionary<string, string?> GetMessages()
        {
            if (_messages == null || _messages.Count < 1)
            {
                throw new ConfigurationException("Messages not found, please contact your Administrator to configure our Cash Register");                                
            }
            return _messages;
        }
        /// <summary>
        /// Method to retrieve available countries from configuration
        /// </summary>
        /// <returns>The <see cref="List<Country>"/>.</returns>
        public List<Country> GetCountries()
        {
            List<Country>? countries = _configuration.GetSection("countries").Get<List<Country>>();
            if (countries == null || countries.Count < 1)
            {
                throw new ConfigurationException(_messages.GetValueOrDefault("countriesNotFound"));
            }

            return countries;
        }
    }
}
