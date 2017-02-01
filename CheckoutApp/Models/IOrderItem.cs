namespace CheckoutApp.Models
{
    public interface IOrderItem
    {
        string ItemId();
        string Description();
        int UnitPrice();
        int PriceForQuantity(int quantity);
    }
}