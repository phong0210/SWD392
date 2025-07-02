namespace DiamondShopSystem.BLL.Domain.Enums
{
    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Shipped,
        Delivered,
        Canceled
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }

    public enum DeliveryStatus
    {
        Pending,
        Dispatched,
        InTransit,
        Delivered,
        Failed
    }

    public enum TransactionType
    {
        Earned,
        Redeemed
    }
}