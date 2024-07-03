using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConducterSO.Agents.SoftwareCompany
{
    public class SoftwareEngineerAgent
    {
        private const string AgentName = "Software Engineer Agent";
        private const string AgentDescription = @"You are Software Engieer, and your goal is develop web app using HTML 
                                                  and JavaScript (JS) by taking into consideration all the requirements given by Program Manager.";

        private readonly Kernel _kernel;

        public SoftwareEngineerAgent(Kernel kernel) =>
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
                    break;
                }

                chat.Add(new ChatMessageContent(content.Role, content.Content));
            }
        }
    }
}
