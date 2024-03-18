namespace WebApplicationApi.DTO;

public class CreateDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string PhoneNumber { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}