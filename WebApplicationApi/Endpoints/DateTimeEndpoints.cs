using WebApplicationApi.Interface;

namespace WebApplicationApi.Endpoints;

public static class DateTimeEndpoints
{
    public static void MapDateTimeEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/datetime", GetDateTime)
            .WithName("GetDateTime")
            .WithOpenApi();
    }

    private static IResult GetDateTime(ITimeService timeService)
    {
        var dateTime = timeService.GetDateTime();
        return Results.Ok(dateTime.ToLongDateString());
    }
}