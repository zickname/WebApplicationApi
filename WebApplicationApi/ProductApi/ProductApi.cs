using Npgsql;
using WebApplicationApi.ProductCURL;

namespace WebApplicationApi.ProductCURL;

public static class ProductApi
{
    public static async Task<List<Product>> GetAll(IConfiguration configuration)
    {
        var products = new List<Product>();
        var connectionString = configuration["ConnectionStrings"].ToString();
        using var connection = new NpgsqlConnection(connectionString);
    
        await connection.OpenAsync();
    
        var commandText = "SELECT * FROM products WHERE is_deleted = false";
        using var command = new NpgsqlCommand(commandText, connection);
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var product = new Product
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                Description = reader.GetString(reader.GetOrdinal("description")),
                Price = reader.GetDouble(reader.GetOrdinal("price")),
                CreateDate = reader.GetDateTime(reader.GetOrdinal("create_date"))
            };
            products.Add(product);
        }
    
        await connection.CloseAsync();
    
        return products;
    }
        
    public static async Task<Product> GetById(int id, IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionStrings"].ToString();
        using var connection = new NpgsqlConnection(connectionString);
    
        await connection.OpenAsync();
    
        var commandText = "SELECT * FROM products WHERE id = @id AND is_deleted = false";
    
        using var command = new NpgsqlCommand(commandText, connection);
    
        command.Parameters.AddWithValue("@id", id);
    
        using var reader = await command.ExecuteReaderAsync();
    
        while (await reader.ReadAsync())
        {
            var product = new Product
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                Description = reader.GetString(reader.GetOrdinal("description")),
                Price = reader.GetDouble(reader.GetOrdinal("price")),
                CreateDate = reader.GetDateTime(reader.GetOrdinal("create_date"))
            };
    
            return product;
        }
    
        return null;
    }
    //
    // public static async Task<IResult> CreateProduct(Product product, IConfiguration configuration)
    // {
    //     var connectionString = configuration["ConnectionStrings"].ToString();
    //     using var connection = new NpgsqlConnection(connectionString);
    //
    //     await connection.OpenAsync();
    //
    //     var commandText =
    //         "INSERT INTO products (name, description, price) VALUES (@name, @description, @price) RETURNING id";
    //
    //     using var command = new NpgsqlCommand(commandText, connection);
    //
    //     command.Parameters.AddWithValue("@name", product.Name);
    //     command.Parameters.AddWithValue("@description", product.Description);
    //     command.Parameters.AddWithValue("@price", product.Price);
    //
    //     var response = await command.ExecuteScalarAsync();
    //
    //     return Results.Ok(response);
    // }
    //
    // public static async Task<IResult> UpdateProduct(int id, Product product, IConfiguration configuration)
    // {
    //     var connectionString = configuration["ConnectionStrings"].ToString();
    //     using var connection = new NpgsqlConnection(connectionString);
    //
    //     await connection.OpenAsync();
    //
    //     var commandText = @"UPDATE products 
    //                         SET name = @name, 
    //                             description = @description, 
    //                             price = @price, 
    //                             last_modified_date = @last_modified_date 
    //                         WHERE id = @id";
    //
    //     using var command = new NpgsqlCommand(commandText, connection);
    //
    //     command.Parameters.AddWithValue("@id", id);
    //     command.Parameters.AddWithValue("@name", product.Name);
    //     command.Parameters.AddWithValue("@description", product.Description);
    //     command.Parameters.AddWithValue("@price", product.Price);
    //     command.Parameters.AddWithValue("@last_modified_date", DateTimeOffset.Now);
    //
    //     await command.ExecuteNonQueryAsync();
    //
    //     return Results.Ok();
    // }
    //
    // public static async Task<IResult> DeleteProduct(int id, IConfiguration configuration)
    // {
    //     var connectionString = configuration["ConnectionStrings"].ToString();
    //     using var connection = new NpgsqlConnection(connectionString);
    //
    //     await connection.OpenAsync();
    //
    //     var commandText = @"UPDATE products 
    //                         SET deleted_date = @deleted_date, 
    //                             is_deleted = @isDeleted 
    //                         WHERE id = @id AND  is_deleted = false";
    //
    //     await using var command = new NpgsqlCommand(commandText, connection);
    //
    //     command.Parameters.AddWithValue("@id", id);
    //     command.Parameters.AddWithValue("@is_deleted", true);
    //     command.Parameters.AddWithValue("@deleted_date", DateTimeOffset.Now);
    //
    //     await command.ExecuteNonQueryAsync();
    //
    //     return Results.Ok();
    // }
}