using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationApi.Models;

[Table("products")]
public class Product
{
    [Column("id")] public int Id { get; set; }

    [Column("name")] [MaxLength(200)] public string Name { get; set; } = null!;

    [Column("create_date")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreateDate { get; set; }

    [Column("deleted_date")] public DateTime? DeletedDate { get; set; }

    [Column("is_deleted")] public bool IsDeleted { get; set; }

    [Column("price")] public double Price { get; set; }

    [Column("last_modified_date")] public DateTime? LastModifiedDate { get; set; }

    [Column("description")] public string? Description { get; set; } = null!;
}