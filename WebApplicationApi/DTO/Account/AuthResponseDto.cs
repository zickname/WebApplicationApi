namespace WebApplicationApi.DTO.Account;

public class AuthResponseDto
{
    public int Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string MiddleName { get; init; }
    public required string PhoneNumber { get; init; }
}