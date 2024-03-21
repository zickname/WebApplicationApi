using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationApi.Models;

[Table("products")]
public class Product
{
    [Column("id")]
    public int Id { get; init; }

    [Column("name")]
    [MaxLength(200)]
    public required string Name { get; set; }

    [Column("create_date")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreateDate { get; } = DateTime.UtcNow;

    [Column("deleted_date")]
    public DateTime? DeletedDate { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [Column("price")]
    public double Price { get; set; }

    [Column("last_modified_date")]
    public DateTime? LastModifiedDate { get; set; }

    [Column("description")]
    [MaxLength(int.MaxValue)]
    public required string Description { get; set; }
}