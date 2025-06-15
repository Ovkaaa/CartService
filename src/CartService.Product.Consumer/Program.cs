using CartService.BLL;
using CartService.DAL;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddBLL()
    .AddDAL(builder.Configuration);

builder.Build().Run();
