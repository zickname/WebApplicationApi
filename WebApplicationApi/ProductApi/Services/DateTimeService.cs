using WebApplicationApi.ProductApi.Interface;

namespace WebApplicationApi.ProductApi.Services;

public class DateTimeService : ITimeService
{
    public string GetDateTime() => DateTime.Now.ToLongDateString();
}