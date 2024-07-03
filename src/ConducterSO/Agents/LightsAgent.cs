using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConducterSO.Agents
{
    public class LightsAgent
    {
        private const string AgentName = "LightsAgent";
        private const string AgentDescription = "Answer questions about lights";
        private readonly Kernel _kernel;

        public LightsAgent(Kernel kernel) =>
            _kernel = kernel;

        public async Task InvokeAgentAsync(ChatHistory chat, string input)
        {
            ChatCompletionAgent agent = new()
            {
                Name = AgentName,
                Instructions = AgentDescription,
                Kernel = _kernel
            };

            await foreach (var content in agent.InvokeAsync(chat))
            {
                var agentResponse = content.ToString();
                if (string.IsNullOrEmpty(agentResponse))
                {
                    continue;
                }

                chat.Add(new ChatMessageContent(content.Role, content.Content));
            }
        }

        public async Task UseSingleChatCompletionAgentAsync()
        {
            ChatCompletionAgent agent = new()
            {
                Name = AgentName,
                Instructions = AgentDescription,
                Kernel = _kernel
            };

            // Local function to invoke agent and display the conversation messages.
            async Task InvokeAgentAsync(ChatHistory chat, string input)
            {
                chat.Add(new ChatMessageContent(AuthorRole.User, input));
                Console.WriteLine($"# {AuthorRole.User}: '{input}'");

                await foreach (var content in agent.InvokeAsync(chat))
                {
                    Console.WriteLine($"# {content.Role} - {content.AuthorName ?? "*"}: '{content.Content}'");
                }
            }
        }
    }
}
