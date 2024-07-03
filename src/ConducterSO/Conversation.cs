using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ConducterSO
{
    public static class Conversation
    {
        /// <summary>
        /// Starts a new chat session
        /// </summary>
        public async static Task StartChatAsync(this HostApplicationBuilder builder)
        {
            /**
        * Enable automatic function calling
        * Function calling is a way the AI can invoke functions with the corrects parameters.
*/
            OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

            var srvProvider = builder.Services.BuildServiceProvider();
            Console.WriteLine("Chat content:");
            Console.WriteLine("------------------------");

            // Create a history store the conversation
            var history = new ChatHistory();

            string? userInput;
            var chatCompletitionService = srvProvider.GetRequiredService<IChatCompletionService>();
            var kernelService = srvProvider.GetRequiredService<Kernel>();
            do
            {
                // Collect user input
                Console.Write("User > ");
                userInput = Console.ReadLine();

                // Add user input
                history.AddUserMessage(userInput);

                var result = await chatCompletitionService.GetChatMessageContentAsync(
                    history,
                    executionSettings: openAIPromptExecutionSettings,
                    kernel: kernelService);

                // Add the message from the agent to the chat history
                history.AddMessage(result.Role, result.Content ?? string.Empty);

                await MessageOutputAsync(history);
            } while (userInput is not null);
        }

        /// <summary>
        /// Outputs the last message of the chat history
        /// </summary>
        public static Task MessageOutputAsync(ChatHistory chatHistory)
        {
            var message = chatHistory.Last();

            Console.WriteLine($"{message.Role} > {message.Content}");
            Console.WriteLine("------------------------");

            return Task.CompletedTask;
        }
    }
}
