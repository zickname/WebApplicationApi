using Microsoft.EntityFrameworkCore;
using WebApplicationApi.Data;
using WebApplicationApi.DTO;
using WebApplicationApi.Models;
using static BCrypt.Net.BCrypt;

namespace WebApplicationApi.Endpoints;

public static class AccountsEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("accounts/create", CreateUserAccount)
            .WithOpenApi();
        endpoints.MapPost("accounts/authenticate", AuthUserAccount)
            .WithOpenApi();
    }

    private static async Task<IResult> CreateUserAccount(AccountCreateDto accountCreateDto, AppDbContext db)
    {
        var hashedPassword = HashPassword(accountCreateDto.Password);
        var account = new Account()
        {
            FirstName = accountCreateDto.FirstName,
            MiddleName = accountCreateDto.MiddleName,
            LastName = accountCreateDto.LastName,
            PhoneNumber = accountCreateDto.PhoneNumber,
            Password = hashedPassword,
            Login = accountCreateDto.Login
        };
        await db.Accounts.AddAsync(account);
        
        await db.SaveChangesAsync();

        return Results.Ok(account.Id);
    }

    private static async Task<AccountGetDto?> AuthUserAccount(AuthAccount authAccount, AppDbContext db)
    {
        var account = await db.Accounts.FirstOrDefaultAsync(account => account.Login == authAccount.Login);

        if (account != null && Verify(authAccount.Password, account.Password))
        {
            return new AccountGetDto()
            {
                Id = account.Id,
                FirstName = account.FirstName,
                LastName = account.LastName,
                MiddleName = account.MiddleName,
                PhoneNumber = account.PhoneNumber
            };
        }

        return null;
    }
}