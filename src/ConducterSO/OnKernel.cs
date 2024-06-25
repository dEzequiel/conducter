using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

namespace ConducterSO
{
    public static class OnKernel
    {
        public static Kernel BuildKernelOnTopOfOpenApi(IConfiguration configuration)
        {
            var openApiModel = configuration["OpenApi:model"];
            var openApiKey = configuration["OpenApi:key"];

            var builder = Kernel.CreateBuilder()
                .AddOpenAIChatCompletion(openApiModel, openApiKey);

            return builder.Build();
        }
    }
}
