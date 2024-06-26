namespace ConducterSO.Models
{
    public class Discount
    {
        public decimal Price { get; private set; }

        public Discount(decimal price)
        {
            Price = price;
        }

        public void ApplyProductDiscount(Product product)
        {
            product.SetPrice(product.Price - Price);
        }

        public void SetPrice(decimal price)
        {
            this.Price = price;
        }
    }
}
