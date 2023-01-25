namespace CashRegisterCore.Models
{
    // Country Model to bind available countries
    public class Country
    {
        //Name of country
        public string Name { get; set; } = "";
        // Bills and coins (we can't distinguish from bills and coins but is not necessary) available for country
        public float[] BillsAndCoins { get; set; } = Array.Empty<float>();
    }
}
