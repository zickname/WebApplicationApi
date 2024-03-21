namespace WebApplicationApi.DTO.Account;

public class AuthRequestDto
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
}