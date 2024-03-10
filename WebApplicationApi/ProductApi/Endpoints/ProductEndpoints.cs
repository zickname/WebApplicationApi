using System.Runtime.CompilerServices;
using static WebApplicationApi.ProductCURL.ProductApi;

namespace WebApplicationApi.ProductCURL.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder endpoints, IConfiguration configuration)
    {
        endpoints.MapGet("api/products", GetAll)
            .WithName("GetProducts")
            .WithOpenApi();

        endpoints.MapGet("api/product/{id}", GetById)
            .WithName("GetProduct")
            .WithOpenApi();

        endpoints.MapPost("api/products", CreateProduct)
            .WithName("AddProduct")
            .WithOpenApi();

        endpoints.MapPut("api/products/{id}", UpdateProduct)
            .WithName("UpdateProduct")
            .WithOpenApi();

        endpoints.MapDelete("api/product/{id}", DeleteProduct)
            .WithName("DeleteProduct")
            .WithOpenApi();
    }
}