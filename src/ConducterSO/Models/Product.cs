namespace ConducterSO.Models
{
    public class Product
    {
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public List<Discount> Discounts { get; private set; } = new();

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public void AddDiscount(Discount discount)
        {
            this.Discounts.Add(discount);
        }
        
        public void AddDiscounts(IEnumerable<Discount> discounts)
        {
            this.Discounts.AddRange(discounts);
        }

        public void ApplyDiscountsToProduct()
        {
            foreach (var discount in this.Discounts)
            {
                discount.ApplyProductDiscount(this);
            }
        }

        public void SetPrice(decimal price)
        {
            this.Price = price;
        }
    }
}
