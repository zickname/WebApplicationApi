using Newtonsoft.Json;
using WebApplication1;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=24326234";
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



app.MapGet("api/products", async (HttpContext context) =>
    {
        var products = new List<Product>();
        using (var connection = new NpgsqlConnection(connectionString))
        {
            await connection.OpenAsync();

            var commandText = "SELECT * FROM products WHERE is_deleted = false";

            using (var command = new NpgsqlCommand(commandText, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var product = new Product
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Description = reader.GetString(reader.GetOrdinal("description")),
                            Price = reader.GetDouble(reader.GetOrdinal("price")),
                            CreateDate = reader.GetDateTime(reader.GetOrdinal("create_date")),
                            LastModifiedDate = reader.GetDateTime(reader.GetOrdinal("last_modified_date"))
                        };
                        products.Add(product);
                    }
                }
            }
        }

        await context.Response.WriteAsJsonAsync(products);
    })
    .WithName("GetProducts")
    .WithOpenApi();

app.MapGet("api/product/{id}", async (int id, HttpContext context) =>
    {
        await using (var connection = new NpgsqlConnection(connectionString))
        {
            await connection.OpenAsync();

            var commandText = "SELECT * FROM products WHERE id = @id AND is_deleted = false";

            await using (var command = new NpgsqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                await using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var product = new Product
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Description = reader.GetString(reader.GetOrdinal("description")),
                            Price = reader.GetDouble(reader.GetOrdinal("price")),
                            CreateDate = reader.GetDateTime(reader.GetOrdinal("create_date")),
                            LastModifiedDate = reader.GetDateTime(reader.GetOrdinal("last_Modified_date"))
                        };
                        
                        await context.Response.WriteAsJsonAsync(product);
                    }
                }
            }
        }

    })
    .WithName("GetProduct")
    .WithOpenApi();

app.MapPost("api/products", async (Product product) =>
    {
        await using (var connection = new NpgsqlConnection(connectionString))
        {
            await connection.OpenAsync();

            var commandText =
                "INSERT INTO products (name, description, price) VALUES (@name, @description, @price) RETURNING id";

            await using (var command = new NpgsqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@name", product.Name);
                command.Parameters.AddWithValue("@description", product.Description);
                command.Parameters.AddWithValue("@price", product.Price);

                var response = await command.ExecuteScalarAsync();
                return Results.Ok(response);
            }
        }
    })
    .WithName("AddProduct")
    .WithOpenApi();

app.MapPut("api/products/{id}", async (int id, Product product) =>
    {
        await using (var connection = new NpgsqlConnection(connectionString))
        {
            await connection.OpenAsync();

            var commandText = @"UPDATE products 
                            SET name = @name, 
                                description = @description, 
                                price = @price, 
                                last_modified_date = @last_modified_date 
                            WHERE id = @id";

            using (var command = new NpgsqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", product.Name);
                command.Parameters.AddWithValue("@description", product.Description);
                command.Parameters.AddWithValue("@price", product.Price);
                command.Parameters.AddWithValue("@last_modified_date", DateTimeOffset.Now);

                await command.ExecuteNonQueryAsync();
            }
        }

        return Results.NoContent();
    })
    .WithName("UpdateProduct")
    .WithOpenApi();

app.MapDelete("api/product/{id}", async (int id) =>
    {
        await using (var connection = new NpgsqlConnection(connectionString))
        {
            await connection.OpenAsync();

            var commandText = @"UPDATE products 
                            SET deleted_date = @deleted_date, 
                                is_deleted = @isDeleted 
                            WHERE id = @id AND  is_deleted = false";

            await using (var command = new NpgsqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@is_deleted", true);
                command.Parameters.AddWithValue("@deleted_date", DateTimeOffset.Now);
                await command.ExecuteNonQueryAsync();
            }
        }

        return Results.Ok();
    })
    .WithName("DeleteProduct")
    .WithOpenApi();

app.Run();
