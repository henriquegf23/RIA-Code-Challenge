using customers_manager.Models;
using customers_manager.Models.Interfaces;
using customers_manager.Repository;
using customers_manager.Repository.Interfaces;
using customers_manager.Services;
using customers_manager.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);


string contentRootPath = builder.Environment.ContentRootPath;
string dataFolder = Path.Combine(contentRootPath, "App_Data");
Directory.CreateDirectory(dataFolder);

string filePath = Path.Combine(dataFolder, "data_store.json");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICustomerServices, CustomerServices>();

//string memoryFile = AppDomain.CurrentDomain.BaseDirectory + "data_store.json";
builder.Services.AddScoped<IPersistData>(s => new PersistData(contentRootPath));

builder.Services.AddScoped<ICustomer, Customer>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
