using WebApplicationApi.Interface;

namespace WebApplicationApi.Services;

public class DateTimeService : ITimeService
{
    public DateTime GetDateTime() => DateTime.Now;
}