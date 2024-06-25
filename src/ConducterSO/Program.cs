using ConducterSO;
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

OnKernel.BuildKernelOnTopOfOpenApi(configuration);
