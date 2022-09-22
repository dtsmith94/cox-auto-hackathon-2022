using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

[assembly: FunctionsStartup(typeof(functions.Startup))]

namespace functions {
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Note: Only register dependencies, do not depend or request those in Configure().
            // Dependencies are only usable during function execution, not before (like here).
            
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton(provider => {
                return new ComputerVisionClient(new ApiKeyServiceClientCredentials("c6a994cd68c14b1ea68267c25c11c479")){ Endpoint = "https://hackathon-22.cognitiveservices.azure.com/"};
            });
            builder.Services.AddTransient<ICognitiveServicesHelper, CognitiveServicesHelper>();

            // builder.Services.AddSingleton<ILoggerProvider, MyLoggerProvider>();
        }
    }
}
