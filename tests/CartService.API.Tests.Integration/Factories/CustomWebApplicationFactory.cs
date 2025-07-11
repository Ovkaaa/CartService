using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace CartService.API.Tests.Integration.Factories;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string appsettingsFileName = "appsettings.json";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            string appSettingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, appsettingsFileName);

            if (!File.Exists(appSettingsFilePath))
            {
                throw new FileNotFoundException($"Configuration file '{appSettingsFilePath}' not found.");
            }

            configBuilder.AddJsonFile(appSettingsFilePath, optional: false, reloadOnChange: true);
            configBuilder.AddEnvironmentVariables();
        });
    }
}
