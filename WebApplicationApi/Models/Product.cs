namespace WebApplicationApi;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? DeletedDate { get; set; }
    public bool IsDeleted { get; set; }
    public double Price { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public string Description { get; set; }
}