namespace DiamondShopSystem.BLL.Domain.ValueObjects
{
    public record Address(
        string Street,
        string City,
        string State,
        string PostalCode,
        string Country
    );

    public record Money(
        decimal Amount,
        string Currency // e.g., "VND"
    );

    public class DiamondProperties
    {
        public decimal Carat { get; set; }
        public string Color { get; set; }
        public string Clarity { get; set; }
        public string Cut { get; set; }
    }
}
