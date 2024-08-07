﻿using ConducterSO.Models;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace ConducterSO.Plugins
{
    public class ShopPlugin
    {
        public string Name { get; private set; }
        public List<Product> _products { get; private set; } = new();
        private List<Basket> _baskets { get; set; } = new();

        public ShopPlugin(string name)
        {
            Name = name;
        }

        [KernelFunction]
        public Basket? GetBasketByName(string name)
        {
            return _baskets.Find(x => x.Name == name);
        }

        [KernelFunction("get_baskets")]
        [Description("Get baskets from the shop")]
        public List<Basket> GetBaskets()
        {
            return _baskets;
        }

        [KernelFunction("get_products")]
        [Description("Get products from the shop")]
        public List<Product> GetProducts()
        {
            return _products;
        }

        [KernelFunction("add_product_to_shop")]
        [Description("Add a new product to the shop catalog.")]
        public void AddProductToShop(Product product)
        {
            _products.Add(product);
        }

        [KernelFunction("add_products_to_shop")]
        [Description("Add a collection of products to the shop catalog.")]
        public void AddProductsToShop(IEnumerable<Product> products)
        {
            _products.AddRange(products);
        }

        [KernelFunction("add_baskets_to_shop")]
        [Description("Add a collection of baskets to the shop account.")]
        public void AddBasketsToShop(List<Basket> baskets)
        {
            _baskets.AddRange(baskets);
        }
    }
}
