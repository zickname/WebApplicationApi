using System.Data;
using Npgsql;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApplicationApi.Data;

namespace WebApplicationApi.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/products", GetAll)
            .WithName("GetProducts")
            .WithOpenApi();

        endpoints.MapGet("api/product/{id:int}", GetById)
            .WithName("GetProduct")
            .WithOpenApi();

        endpoints.MapPost("api/products", CreateProduct)
            .WithName("AddProduct")
            .WithOpenApi();

        endpoints.MapPut("api/products/{id:int}", UpdateProduct)
            .WithName("UpdateProduct")
            .WithOpenApi();

        endpoints.MapDelete("api/product/{id:int}", DeleteProduct)
            .WithName("DeleteProduct")
            .WithOpenApi();
    }

    private static async Task<List<ProductDTO>> GetAll(AppDbContext db)
    {
        return await db.Products.ToListAsync();
    }


    private static async Task<ProductDTO?> GetById(int id, AppDbContext db)
    {
        return await db.Products.FirstOrDefaultAsync( p => p.Id == id );
    }

    private static async Task<IResult> CreateProduct(ProductDTO productDto, AppDbContext db)
    {
        await db.Products.AddAsync(productDto);
        await db.SaveChangesAsync();
        return Results.Ok(productDto.Id);
    }
    // private static async Task<IResult> CreateProduct(ProductDTO productDto, IConfiguration configuration)
    // {
    //     var connectionString = configuration["ConnectionStrings"];
    //     
    //     using IDbConnection db = new NpgsqlConnection(connectionString);
    //
    //     var sqlQuery =
    //         "INSERT INTO products (name, description, price) VALUES (@name, @description, @price) RETURNING id";
    //
    //     var userId = await db.ExecuteScalarAsync(sqlQuery, productDto);
    //
    //     return Results.Ok(userId);
    // }

    private static async Task<IResult> UpdateProduct(int id, ProductDTO productDto, IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionStrings"];
        productDto.LastModifiedDate = DateTime.Now;
        
        using IDbConnection db = new NpgsqlConnection(connectionString);

        var sqlQuery = @"UPDATE products 
                            SET name = @name, 
                                description = @description, 
                                price = @price, 
                                last_modified_date = @last_modified_date 
                            WHERE id = @id";

        await db.QueryAsync<ProductDTO>(sqlQuery, new { id, product = productDto });

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