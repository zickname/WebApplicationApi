using System.Runtime.CompilerServices;
using static WebApplicationApi.ProductCURL.ProductApi;

namespace WebApplicationApi.ProductCURL.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder endpoints, IConfiguration configuration)
    {
        endpoints.MapGet("api/products",()=> GetAll(configuration))
            .WithName("GetProducts")
            .WithOpenApi();

        // endpoints.MapGet("api/product/{id}", ProductApi.GetById)
        //     .WithName("GetProduct")
        //     .WithOpenApi();
        //
        // endpoints.MapPost("api/products", ProductApi.CreateProduct)
        //     .WithName("AddProduct")
        //     .WithOpenApi();
        //
        // endpoints.MapPut("api/products/{id}", ProductApi.UpdateProduct)
        //     .WithName("UpdateProduct")
        //     .WithOpenApi();
        //
        // endpoints.MapDelete("api/product/{id}", ProductApi.DeleteProduct)
        //     .WithName("DeleteProduct")
        //     .WithOpenApi();
    }
}