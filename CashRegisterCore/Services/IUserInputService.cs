using CashRegisterCore.Models;

namespace CashRegisterCore.Services
{
    public interface IUserInputService
    {
        /// <summary>
        /// This method retrieve the selected country by user, the service asks for a valid country
        /// </summary>
        /// <param name="countries">The <see cref="List<Country>"/> Available countries to select country</param>
        /// <returns>The <see cref="int"/> For selected country.</returns>
        int SelectCountry(List<Country> countries);
        /// <summary>
        /// This method retrieve the amount to pay for user
        /// </summary>
        /// <returns>The <see cref="float"/> amount to pay.</returns>
        float GetAmountToPay();
        /// <summary>
        /// This method retrieve the bills entered by user
        /// </summary>
        /// <param name="countries">The <see cref="List<Country>"/> Available countries to select country</param>
        /// <param name="selectedCountry">The <see cref="List<Country>"/> Selected country by user</param>
        /// <param name="amountToPay">The <see cref="List<Country>"/> Amount to pay by user</param>
        /// <returns>The <see cref="float[]"/> List bills provided by user.</returns>
        float[] GetBillsFromUser(List<Country> countries, int selectedCountry, float amountToPay);
    }
}
