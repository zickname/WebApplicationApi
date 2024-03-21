namespace WebApplicationApi.DTO.Account;

public abstract class CreateDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string MiddleName { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Login { get; init; }
    public required string Password { get; init; }
}