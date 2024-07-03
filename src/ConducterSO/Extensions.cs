using ConducterSO.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace ConducterSO
{
    public static class Extensions
    {
        // https://github.com/microsoft/semantic-kernel/blob/main/dotnet/samples/Concepts/DependencyInjection/Kernel_Building.cs
        public static void RegisterApplicationSemanticKernel(this HostApplicationBuilder builder)
        {
            RegisterKernelPluginsCollection(builder);
            RegisterChatCompletion(builder);

            builder.Services.AddTransient<Kernel>((srvProvider) =>
            {
                KernelPluginCollection pluginCollection = srvProvider.GetRequiredService<KernelPluginCollection>();
                return new Kernel(srvProvider, pluginCollection);
            });

            _ = builder.Services.BuildServiceProvider().GetRequiredService<Kernel>();
            _ = builder.Services.BuildServiceProvider().GetRequiredService<IChatCompletionService>();
        }

        private static void RegisterKernelPluginsCollection(this HostApplicationBuilder builder)
        {
            builder.Services.AddTransient<KernelPluginCollection>((srvProvider) =>
            {
                KernelPluginCollection plugins = [];
                plugins.AddFromType<LightsPlugin>();
                plugins.AddFromType<ShopPlugin>();
                return plugins;
            }); 
        }

        private static void RegisterChatCompletion(this HostApplicationBuilder builder)
        {
            var openApiModel = builder.Configuration["OpenApi:model"];
            var openApiKey = builder.Configuration["OpenApi:key"];
            builder.Services.AddOpenAIChatCompletion(modelId: openApiModel, apiKey: openApiKey);
        }

    }
}
