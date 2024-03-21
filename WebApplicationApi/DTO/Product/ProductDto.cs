namespace WebApplicationApi.DTO.Product;

public class ProductDto
{
    public int Id { get; set; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public double Price { get; init; }
}