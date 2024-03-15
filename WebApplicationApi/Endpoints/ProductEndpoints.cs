using Microsoft.EntityFrameworkCore;
using WebApplicationApi.Data;
using WebApplicationApi.DTO;
using WebApplicationApi.Models;

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

    private static async Task<List<ProductDto>> GetAll(AppDbContext db)
    {
        return await db.Products.Select(product => new ProductDto
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price
        }).ToListAsync();
    }


    private static async Task<ProductDto?> GetById(int id, AppDbContext db)
    {
        return await db.Products
            .Where(product => product.Id == id)
            .Select(product => new ProductDto
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            })
            .FirstOrDefaultAsync();
    }

    private static async Task<IResult> CreateProduct(ProductDto productDto, AppDbContext db)
    {
        var product = new Product
        {
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
        };

        await db.Products.AddAsync(product);

        await db.SaveChangesAsync();

        return Results.Ok(product.Id);
    }

    private static async Task<IResult> UpdateProduct(int id, ProductDto productDto, AppDbContext db)
    {
        var existingProduct = await db.Products.FindAsync(id);
    
        if (existingProduct == null)
        {
            return Results.NotFound($"Запись с таким {id} не найдена");
        }
    
        existingProduct.Name = productDto.Name;
        existingProduct.Description = productDto.Description;
        existingProduct.Price = productDto.Price;
        existingProduct.LastModifiedDate = DateTime.UtcNow;
    
        db.Products.Update(existingProduct);
    
        await db.SaveChangesAsync();
    
        return Results.Ok(id);
    }
    
    private static async Task<IResult> DeleteProduct(int id, AppDbContext db)
    {
        var existingProduct = await db.Products.FindAsync(id);
    
        if (existingProduct == null)
        {
            return Results.NotFound($"Запись с таким {id} не найдена");
        }
    
        existingProduct.IsDeleted = true;
        existingProduct.DeletedDate = DateTime.UtcNow;
    
        db.Products.Update(existingProduct);
    
        await db.SaveChangesAsync();
    
        return Results.Ok();
    }
}