using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchestraterAI.Config
{
    public static class ServiceCollectionExtension
    {
        public static void AddOrchestraterAIServices(this IServiceCollection services, string deploymentName, string endpoint, string apiKey)
        {
            services.AddScoped<IKernelBuilder>(
            sp =>
            {
                IKernelBuilder builder = Kernel.CreateBuilder();

                builder.AddAzureOpenAIChatCompletion(deploymentName!, endpoint!, apiKey!);
 
                return builder;
            });

            services.AddScoped<IReportProducer, ReportProducer>();
        }
    }
}
