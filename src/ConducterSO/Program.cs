using ConducterSO;
using ConducterSO.Models;
using ConducterSO.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var configurationBuilder = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("config.json", optional: false, reloadOnChange: true);

IConfiguration configuration = configurationBuilder.Build();

var openApiModel = configuration["OpenApi:model"];
var openApiKey = configuration["OpenApi:key"];

IKernelBuilder kernelBuilder = Kernel.CreateBuilder();
kernelBuilder.AddOpenAIChatCompletion(openApiModel, openApiKey);

var myShop = new ShopPlugin("My Shop");

kernelBuilder.Plugins.AddFromType<LightsPlugin>("Lights");
kernelBuilder.Plugins.AddFromType<TestDrivenDevelopmentPlugin>("TDD");
kernelBuilder.Plugins.AddFromObject(myShop);

var kernel = kernelBuilder.Build(); 

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

// Enable planning
//OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
//{
//    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
//};

/**
 * Enable automatic function calling
 * Function calling is a way the AI can invoke functions with the corrects parameters.
 */
OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
{
    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
};

// Create a history store the conversation
var history = new ChatHistory();

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

myShop.AddBasketsToShop([basket1, basket2, basket3]);

myShop.AddProductToShop(product1);
myShop.AddProductToShop(product2);
myShop.AddProductToShop(product3);
myShop.AddProductToShop(product4);

string? userInput;
do
{
    // Collect user input
    Console.Write("User > ");
    userInput = Console.ReadLine();

    // Add user input
    history.AddUserMessage(userInput);

    var result = await chatCompletionService.GetChatMessageContentAsync(
        history,
        executionSettings: openAIPromptExecutionSettings,
        kernel: kernel);

    // Print the results
    Console.WriteLine("Assistant > " + result);

    // Add the message from the agent to the chat history
    history.AddMessage(result.Role, result.Content ?? string.Empty);
} while (userInput is not null);