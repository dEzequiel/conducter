using ConducterSO.Plugins;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace ConducterSO
{
    public static class OnKernel
    {
        // https://github.com/microsoft/semantic-kernel/blob/main/dotnet/samples/Concepts/DependencyInjection/Kernel_Building.cs
        public static void BuildKernelOnTopOfOpenApi(this HostApplicationBuilder builder, IConfiguration configuration)
        {
            var openApiModel = configuration["OpenApi:model"];
            var openApiKey = configuration["OpenApi:key"];

            // Add the OpenAI chat completion service as a singleton
            builder.Services.AddOpenAIChatCompletion(
                modelId: openApiModel,
                apiKey: openApiKey
            );

            //IKernelBuilder kernelBuilder = Kernel.CreateBuilder();
            builder.Services.AddSingleton(() => new LightsPlugin());
            builder.Services.AddSingleton(() => new TestDrivenDevelopmentPlugin());

            // Create the plugin collection (using the KernelPluginFactory to create plugins from objects)
            builder.Services.AddSingleton<KernelPluginCollection>((serviceProvider) =>
                [
                    KernelPluginFactory.CreateFromObject(serviceProvider.GetRequiredService<LightsPlugin>()),
                    KernelPluginFactory.CreateFromObject(serviceProvider.GetRequiredService<TestDrivenDevelopmentPlugin>()),
                    KernelPluginFactory.CreateFromObject(serviceProvider.GetRequiredService<ShopPlugin>())

                ]
            );

            // Finally, create the Kernel service with the service provider and plugin collection
            builder.Services.AddTransient((serviceProvider) => {
                KernelPluginCollection pluginCollection = serviceProvider.GetRequiredService<KernelPluginCollection>();
                return new Kernel(serviceProvider, pluginCollection);
            });

            var kernel = Kernel.CreateBuilder();
            kernel.AddOpenAIChatCompletion(
                modelId: openApiModel,
                apiKey: openApiKey);


           kernel.Build();

        }

        public static async Task StartChatAsync(OpenAIChatCompletionService chat)
        {
            /**
             * Enable automatic function calling
             * Function calling is a way the AI can invoke functions with the corrects parameters.
             */
            OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };


            Console.WriteLine("Chat content:");
            Console.WriteLine("------------------------");

            // Create a history store the conversation
            var history = new ChatHistory();

            string? userInput;
            do
            {
                // Collect user input
                Console.Write("User > ");
                userInput = Console.ReadLine();

                // Add user input
                history.AddUserMessage(userInput);

                //var result = await chat.GetChatMessageContentAsync(
                //    history,
                //    executionSettings: openAIPromptExecutionSettings,
                //    kernel: kernel);
                var result = await chat.GetChatMessageContentAsync(history);
                history.AddMessage(result.Role, result.Content ?? string.Empty);
                // Print the results
                await MessageOutputAsync(history);

                // Add the message from the agent to the chat history
            } while (userInput is not null);
        }


        /// <summary>
        /// Outputs the last message of the chat history
        /// </summary>
        private static Task MessageOutputAsync(ChatHistory chatHistory)
        {
            var message = chatHistory.Last();

            Console.WriteLine($"{message.Role}: {message.Content}");
            Console.WriteLine("------------------------");

            return Task.CompletedTask;
        }
    }
}
