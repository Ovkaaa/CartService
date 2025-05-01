using CartService.API.Tests.Integration.FakeServices;
using CartService.DAL.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CartService.API.Tests.Integration.Factories;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string appsettingsFileName = "appsettings.json";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
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

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IProductCatalogClient));
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddSingleton<IProductCatalogClient, FakeProductCatalogClient>();
        });
    }
}
