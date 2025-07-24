namespace DiamondShopSystem.BLL.Services.Implements.VNPayService.Models
{
    public class PaymentInformationModel
    {
        public string OrderId { get; set; }
        public double Amount { get; set; }
        public string OrderDescription { get; set; }
        public string Name { get; set; }

    }
}