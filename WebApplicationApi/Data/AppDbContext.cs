using Microsoft.EntityFrameworkCore;

namespace WebApplicationApi.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ProductDTO> Products { get; set; } = null!;
    
}