using System.Data;
using Npgsql;
using Dapper;

namespace WebApplicationApi.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder endpoints)
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

    private static async Task<List<Product>> GetAll(IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionStrings"];
        
        using IDbConnection db = new NpgsqlConnection(connectionString);

        var results = await db.QueryAsync<Product>(
            "SELECT * FROM products WHERE is_deleted = false"
        );

        return results.ToList();
    }

    private static async Task<IEnumerable<Product>> GetById(int id, IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionStrings"];
        
        using IDbConnection db = new NpgsqlConnection(connectionString);

        return await db.QueryAsync<Product>(
            "SELECT * FROM products WHERE id = @id AND is_deleted = false",
            new { id });
    }

    private static async Task<IResult> CreateProduct(Product product, IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionStrings"];
        
        using IDbConnection db = new NpgsqlConnection(connectionString);

        var sqlQuery =
            "INSERT INTO products (name, description, price) VALUES (@name, @description, @price) RETURNING id";

        var userId = await db.ExecuteScalarAsync(sqlQuery, product);

        return Results.Ok(userId);
    }

    private static async Task<IResult> UpdateProduct(int id, Product product, IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionStrings"];
        product.LastModifiedDate = DateTime.Now;
        
        using IDbConnection db = new NpgsqlConnection(connectionString);

        var sqlQuery = @"UPDATE products 
                            SET name = @name, 
                                description = @description, 
                                price = @price, 
                                last_modified_date = @last_modified_date 
                            WHERE id = @id";

        await db.QueryAsync<Product>(sqlQuery, new { id, product });

        return Results.Ok();
    }

    private static async Task<IResult> DeleteProduct(int id, IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionStrings"];
        var deletedDate = DateTime.Now;
        var isDeleted = true;
        
        using IDbConnection db = new NpgsqlConnection(connectionString);

        var sqlQuery = @"UPDATE products 
                            SET deleted_date = @deleted_date, 
                                is_deleted = @isDeleted 
                            WHERE id = @id AND  is_deleted = false";

        await db.ExecuteAsync(sqlQuery, new { id, deletedDate, isDeleted });

        return Results.Ok();
    }
}