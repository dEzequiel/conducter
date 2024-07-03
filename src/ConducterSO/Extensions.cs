using ConducterSO.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace ConducterSO
{
    public static class Extensions
    {
        // https://github.com/microsoft/semantic-kernel/blob/main/dotnet/samples/Concepts/DependencyInjection/Kernel_Building.cs
        public static void RegisterApplicationSemanticKernel(this HostApplicationBuilder builder)
        {
            var openApiModel = builder.Configuration["OpenApi:model"];
            var openApiKey = builder.Configuration["OpenApi:key"];

            builder.Services.AddTransient(srv =>
            {
                var builder = Kernel.CreateBuilder();
                builder.RegisterKernelPlugins();
                builder.Services.AddOpenAIChatCompletion(modelId: openApiModel, apiKey: openApiKey);
                return builder.Build();
            });

            _ = builder.Services.BuildServiceProvider().GetRequiredService<Kernel>();
            _ = builder.Services.BuildServiceProvider().GetRequiredService<ShopPlugin>();
        }

        private static void RegisterKernelPlugins(this IKernelBuilder builder)
        {
            builder.Plugins.AddFromType<LightsPlugin>("Lights");
            builder.Plugins.AddFromType<ShopPlugin>("Shop");
        }

    }
}
