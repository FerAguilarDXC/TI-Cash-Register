using CashRegisterCore.Models;

namespace CashRegisterCore.Services
{
    public interface IConfigurationInformationService
    {
        /// <summary>
        /// Method to retrieve messages needed for application from configuration
        /// </summary>
        /// <returns>The <see cref="Dictionary<string, string?>"/>.</returns>
        Dictionary<string, string?> GetMessages();
        /// <summary>
        /// Method to retrieve available countries from configuration
        /// </summary>
        /// <returns>The <see cref="List<Country>"/>.</returns>
        List<Country> GetCountries();
    }
}
