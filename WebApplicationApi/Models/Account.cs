using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationApi.Models;

[Table("accounts")]
public class Account
{
    [Column("id")]
    public int Id { get; init; }

    [Column("first_name")]
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    [MaxLength(50)]
    public string LastName { get; set; } = null!;

    [Column("middle_name")]
    [MaxLength(50)]
    public string MiddleName { get; set; } = null!;

    [Column("password")]
    [MaxLength(100)]
    public string Password { get; set; } = null!;

    [Column("login")]
    [MaxLength(20)]
    public string Login { get; set; } = null!;

    [Column("phone_number")]
    [MaxLength(20)]
    public string PhoneNumber { get; set; } = null!;
}