using CartService.API.Auth;
using CartService.API.Extensions;
using CartService.BLL;
using CartService.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuth(builder.Configuration);

builder.Services.AddBLL();
builder.Services.AddDAL(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapEndpoints();

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program
{ }
