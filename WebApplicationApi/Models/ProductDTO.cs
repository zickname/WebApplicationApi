using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplicationApi;

[Table("products")]
public class ProductDTO
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("create_date")]
    public DateTime CreateDate { get; set; }
    
    [Column("deleted_date")]
    public DateTime? DeletedDate { get; set; }
    
    [Column("is_deleted")]
    public bool IsDeleted { get; set; }
    
    [Column("price")]
    public double Price { get; set; }
    
    [Column("last_modified_date")]
    public DateTime? LastModifiedDate { get; set; }
    
    [Column("description")]
    public string Description { get; set; }
}