namespace CashRegisterCore.Services
{
    public interface ICashRegisterService
    {
        /// <summary>
        /// Whe a user pays an amount we need to give him his change, and to do that we need to provide the 
        /// minimum necessary bills/coins, this method return the list of minimum bills necessary to give the change
        /// </summary>
        /// <param name="amountToPay">The <see cref="float"/> containing the amount to pay.</param>
        /// <param name="userBills">The <see cref="float[]"/> containing the bills provided by user.</param>
        /// <param name="billsAndCoins">The <see cref="float[]"/> containing the bills and coins accepted by our cash register.</param>
        /// <returns>The <see cref="List<float>"/>.</returns>
        List<float> GetMinimumBillsChange(float amountToPay, float[] userBills, float[] billsAndCoins);
    }
}
