using ConducterSO;
using ConducterSO.Models;
using ConducterSO.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Environment.ContentRootPath = Directory.GetCurrentDirectory();
builder.Configuration.AddJsonFile("config.json", optional: false);
builder.Configuration.AddCommandLine(args);
builder.RegisterApplicationSemanticKernel();

Motion.Execute();
await Conversation.StartChatAsync(builder);

IHost host = builder.Build();
host.Run();


static class Motion
{
    public static void Execute()
    {


        var product1 = new Product("Laptop", 999.99m);
        var discountList1 = new List<Discount>
{
    new Discount(10.00m),
    new Discount(15.50m)
};
        product1.AddDiscounts(discountList1);

        var product2 = new Product("Smartphone", 499.49m);
        var discountList2 = new List<Discount>
{
    new Discount(20.00m),
    new Discount(5.00m)
};
        product2.AddDiscounts(discountList2);

        var product3 = new Product("Tablet", 299.99m);
        var discountList3 = new List<Discount>
{
    new Discount(7.25m),
    new Discount(12.75m)
};
        product3.AddDiscounts(discountList3);

        var product4 = new Product("Smartwatch", 199.99m);
        var discountList4 = new List<Discount>
{
    new Discount(18.00m),
    new Discount(25.00m)
};
        product4.AddDiscounts(discountList4);

        Basket basket1 = new Basket("Gifts for Family");
        basket1.AddProductToBasket(product1);
        var totalBasketPrice = basket1.SaveBasket();
        Console.WriteLine($"{basket1.Name} total price is {totalBasketPrice.ToString()}");

        Basket basket2 = new Basket("Gifts for Friends");
        basket2.AddProductToBasket(product1);
        totalBasketPrice = basket2.SaveBasket();
        Console.WriteLine($"{basket2.Name} total price is {totalBasketPrice.ToString()}");

        Basket basket3 = new Basket("Gifts for Pets");
        basket3.AddProductToBasket(product1);
        totalBasketPrice = basket3.SaveBasket();
        Console.WriteLine($"{basket3.Name} total price is {totalBasketPrice.ToString()}");

        var myShop = new ShopPlugin();
        myShop.AddBasketsToShop([basket1, basket2, basket3]);
        myShop.AddProductToShop(product1);
        myShop.AddProductToShop(product2);
        myShop.AddProductToShop(product3);
        myShop.AddProductToShop(product4);
    }
}