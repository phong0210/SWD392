namespace DiamondShopSystem.BLL.Models
{
    public class CreateDeliveryDto
    {
        public Guid OrderId { get; set; }
        public string? ShippingAddress { get; set; }
        public int Status { get; set; }
    }
}