namespace WebApplicationApi.DTO.Account;

public abstract class AuthRequestDto
{
    public required string Login { get; init; }
    public required string Password { get; init; }
}