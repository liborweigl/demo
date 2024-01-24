using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using System.Reflection;

namespace OrchestraterAI.Extentions
{
    public static class SemanticKernelExtentions
    {
        /// <summary>
        /// Register plugins with a given _kernel.
        /// </summary>
        public static Task RegisterPluginsAsync(Kernel kernel, string semanticPluginsDirectory)
        {
            var logger = kernel.LoggerFactory.CreateLogger(nameof(Kernel));

            var pluginPath = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(Assembly.GetExecutingAssembly().Location)), semanticPluginsDirectory);

            if (!string.IsNullOrWhiteSpace(semanticPluginsDirectory))
            {
                foreach (string subDir in Directory.GetDirectories(pluginPath))
                {
                    try
                    {
                        kernel.ImportPluginFromPromptDirectory(subDir);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError("Could not load plugin from {Directory}: {Message}", subDir, ex.Message);
                    }
                }
            }



            return Task.CompletedTask;
        }
    }
}
