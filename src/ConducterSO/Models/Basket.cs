namespace ConducterSO.Models
{
    public class Basket
    {
        public string Name { get; private set; }
        public List<Product> Catalog { get; private set; } = new();

        public Basket(string name)
        {
            Name = name;
        }

        public List<Product> GetShopCatalog()
        {
            return this.Catalog;
        }

        public void AddProductToBasket(Product product)
        {
            this.Catalog.Add(product);
        }
        
        public void AddProductsToBasket(IEnumerable<Product> products)
        {
            this.Catalog.AddRange(products);
        }

        public void RemoveProductFromBasket(Product product)
        {
            this.Catalog.Remove(product);
        }

        public void ResetBasket()
        {
            this.Catalog.Clear();
        }

        public decimal SaveBasket()
        {
            if (!this.Catalog.Any())
                return 0;

            foreach (var product in this.Catalog)
            {
                product.ApplyDiscountsToProduct();
            }

            return GetBasketTotalPrice();   
        }

        public decimal GetBasketTotalPrice()
        {
            decimal total = 0;
            foreach (var product in this.Catalog)
            {
                total += product.Price;
            }

            return total;
        }
    }
}
