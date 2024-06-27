using ConducterSO.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;

namespace ConducterSO
{
    public static class OnKernel
    {
        // https://github.com/microsoft/semantic-kernel/blob/main/dotnet/samples/Concepts/DependencyInjection/Kernel_Building.cs
        public static void AddKernel(this HostApplicationBuilder builder)
        {
            var openApiModel = builder.Configuration["OpenApi:model"];
            var openApiKey = builder.Configuration["OpenApi:key"];

            builder.Services.AddKernel().AddOpenAIChatCompletion(openApiModel, openApiKey);
            builder.Services.AddSingleton<KernelPlugin>(sp => KernelPluginFactory.CreateFromType<LightsPlugin>(serviceProvider: sp));
            builder.Services.AddSingleton<KernelPlugin>(sp => KernelPluginFactory.CreateFromType<ShopPlugin>(serviceProvider: sp));

            _ = builder.Services.BuildServiceProvider().GetRequiredService<Kernel>();
        }

    }
}
