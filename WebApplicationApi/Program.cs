using Newtonsoft.Json;
using WebApplicationApi;
using Npgsql;
using WebApplicationApi.ProductCURL;

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

app.UseHttpsRedirection();

app.MapGet("api/products", ProductApi.GetAll)
    .WithName("GetProducts")
    .WithOpenApi();

app.MapGet("api/product/{id}", ProductApi.GetById)
    .WithName("GetProduct")
    .WithOpenApi();

app.MapPost("api/products", ProductApi.CreateProduct)
    .WithName("AddProduct")
    .WithOpenApi();

app.MapPut("api/products/{id}", ProductApi.UpdateProduct)
    .WithName("UpdateProduct")
    .WithOpenApi();

app.MapDelete("api/product/{id}", ProductApi.DeleteProduct)
    .WithName("DeleteProduct")
    .WithOpenApi();

app.Run();
