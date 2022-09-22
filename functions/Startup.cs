using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(functions.Startup))]

namespace functions {
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Note: Only register dependencies, do not depend or request those in Configure().
            // Dependencies are only usable during function execution, not before (like here).
            
            builder.Services.AddHttpClient();
            // builder.Services.AddSingleton<ILoggerProvider, MyLoggerProvider>();
        }
    }
}
