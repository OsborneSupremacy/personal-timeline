namespace Personal.Timeline.Web.Extensions;

public static class DateTimeExtensions
{
    public static DateOnly ToDateOnly(this DateTime dateTime) =>
        DateOnly.FromDateTime(dateTime);

    public static DateOnly FromDateTime(this DateTime dateTime) =>
        DateOnly.FromDateTime(dateTime);
}