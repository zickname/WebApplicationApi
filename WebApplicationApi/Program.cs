using Newtonsoft.Json;
using WebApplicationApi;
using Npgsql;
using WebApplicationApi.ProductCURL;
using WebApplicationApi.ProductCURL.Data;
using WebApplicationApi.ProductCURL.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Console.WriteLine(app.Configuration.GetConnectionString("DefaultConnection"));

app.UseHttpsRedirection();

app.MapProductEndpoints();

app.Run();