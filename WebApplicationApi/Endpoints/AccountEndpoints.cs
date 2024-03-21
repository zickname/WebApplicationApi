using Microsoft.EntityFrameworkCore;
using WebApplicationApi.Data;
using WebApplicationApi.DTO.Account;
using WebApplicationApi.Models;
using static BCrypt.Net.BCrypt;

namespace WebApplicationApi.Endpoints;

public static class AccountEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("accounts/create", Create)
            .WithOpenApi();

        endpoints.MapPost("accounts/authenticate", Authenticate)
            .WithOpenApi();
    }

    private static async Task<IResult> Create(CreateDto createDto, AppDbContext db)
    {
        var account = await db.Accounts.FirstOrDefaultAsync(account => account.Login == createDto.Login);

        if (account != null)
        {
            return Results.BadRequest("Пользователь с таким логином уже зарегистрирован");
        }
        
        var hashedPassword = HashPassword(createDto.Password);

        var newAccount = new Account()
        {
            FirstName = createDto.FirstName,
            MiddleName = createDto.MiddleName,
            LastName = createDto.LastName,
            PhoneNumber = createDto.PhoneNumber,
            Password = hashedPassword,
            Login = createDto.Login.ToLower()
        };
        
        await db.Accounts.AddAsync(newAccount);

        await db.SaveChangesAsync();

        return Results.Ok(newAccount.Id);
    }

    private static async Task<IResult> Authenticate(AuthRequestDto authRequestDto, AppDbContext db)
    {
        var account = await db.Accounts.FirstOrDefaultAsync(account => account.Login == authRequestDto.Login);

        if (account == null)
        {
            return Results.BadRequest("Пользователь не зарегистрирован");
        }

        if (!Verify(authRequestDto.Password, account.Password))
        {
            return Results.BadRequest("Неправильный пароль");
        }

        var responseDto = new AuthResponseDto()
        {
            Id = account.Id,
            FirstName = account.FirstName,
            LastName = account.LastName,
            MiddleName = account.MiddleName,
            PhoneNumber = account.PhoneNumber
        };

        return Results.Ok(responseDto);
    }
}