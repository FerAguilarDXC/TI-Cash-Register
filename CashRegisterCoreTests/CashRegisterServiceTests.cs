using CashRegisterCore.Services;

namespace CashRegisterCoreTests
{
    public class CashRegisterServiceTests
    {
        private readonly ICashRegisterService _service;
        public CashRegisterServiceTests()
        {
            _service = new CashRegisterService();
        }
        [Fact(DisplayName = "When the user enters a single bill greater than the amount he wants to pay, the correct change is given to him")]
        public void WheUserEntersSingleBillGraterThanAmountToPayThenTheCorrectChangeIsDelivered()
        {
            float amountToPay = 57;
            float[] userBills = new float[] { 100.00f };
            float[] billsAndCoins = new float[] { 0.01f, 0.05f, 0.10f, 0.25f, 0.50f, 1.00f, 2.00f, 5.00f, 10.00f, 20.00f, 50.00f, 100.00f };
            List<float> result = _service.GetMinimumBillsChange(amountToPay, userBills, billsAndCoins);

            Assert.Equal(userBills.Sum() - amountToPay, result.Sum());
            Assert.Equal(4, result.Count);
            Assert.Contains(20.00f, result);
            Assert.Contains(2.00f, result);
            Assert.Contains(1.00f, result);

        }

        [Fact(DisplayName = "When the user enters more than 1 bill and the sum is greater than the amount he wants to pay, then the correct change is given")]
        public void WheUserEntersMoreThanOneBillGraterThanAmountToPayThenTheCorrectChangeIsDelivered()
        {
            float amountToPay = 13.03f;
            float[] userBills = new float[] { 10.00f, 5.00f };
            float[] billsAndCoins = new float[] { 0.01f, 0.05f, 0.10f, 0.25f, 0.50f, 1.00f, 2.00f, 5.00f, 10.00f, 20.00f, 50.00f, 100.00f };
            List<float> result = _service.GetMinimumBillsChange(amountToPay, userBills, billsAndCoins);

            Assert.Equal(1.97, Math.Round(result.Sum(), 2));//1.97
            Assert.Equal(7, result.Count);
            Assert.Contains(1.00f, result);
            Assert.Contains(0.50f, result);
            Assert.Contains(0.25f, result);
            Assert.Contains(0.10f, result);//x2
            Assert.Contains(0.01f, result);//x2

        }
    }
}