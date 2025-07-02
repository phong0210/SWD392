
namespace DiamondShopSystem.BLL.Application.DTOs
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Sku { get; set; }
        public float Price { get; set; }
        public int StockQuantity { get; set; }
        public string GiaCertificationNumber { get; set; }
        public bool IsHidden { get; set; }
        public float Carat { get; set; }
        public string Color { get; set; }
        public string Clarity { get; set; }
        public string Cut { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
