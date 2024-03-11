using WebApplicationApi.ProductApi.Interface;

namespace WebApplicationApi.ProductApi.Endpoints;

public static class DateTimeEndpoints
{
    public static void MapDateTime(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/datetime", GetDateTime)
            .WithName("GetDateTime")
            .WithOpenApi();
    }

    private static async Task<IResult> GetDateTime(ITimeService timeService)
    {
        var dateTime = timeService.GetDateTime();
        return await Task.FromResult(Results.Ok(dateTime));
    }
}