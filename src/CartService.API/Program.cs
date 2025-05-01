using CartService.API.Endpoints;
using CartService.Application;
using CartService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBLL();
builder.Services.AddDAL(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.AddCartEndpoints();

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program
{ }
