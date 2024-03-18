namespace WebApplicationApi.DTO;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public double Price { get; init; }
}