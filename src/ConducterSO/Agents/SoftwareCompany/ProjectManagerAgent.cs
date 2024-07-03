using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;

namespace ConducterSO.Agents.SoftwareCompany
{
    public class ProjectManagerAgent
    {
        private const string AgentName = "Project Manager Agent";
        private const string AgentDescription = @"You are a program manager which will take the requirement and create a plan for creating app. 
                                                Program Manager understands the user requirements and form the detail documents with requirements 
                                                and costing.";

        private readonly Kernel _kernel;

        public ProjectManagerAgent(Kernel kernel) =>
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
