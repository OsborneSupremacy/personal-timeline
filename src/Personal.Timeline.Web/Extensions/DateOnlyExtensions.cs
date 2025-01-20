using System.Globalization;

namespace Personal.Timeline.Web.Extensions;

internal static class DateOnlyExtensions
{
    public static DateOnly Today => DateOnly.FromDateTime(DateTime.Today);

    public static bool IsToday(this DateOnly date) =>
        date.Equals(Today);

    public static bool IsToday(this DateOnly? date) =>
        date.HasValue && date.Value.IsToday();

    public static bool IsPast(this DateOnly date) =>
        date < Today;

    public static bool IsPast(this DateOnly? date) =>
        date.HasValue && date.Value.IsPast();

    public static bool IsFuture(this DateOnly date) =>
        date > Today;

    public static bool IsFuture(this DateOnly? date) =>
        date.HasValue && date.Value.IsFuture();

    public static bool IsIndefinite(this DateOnly? date) =>
        !date.HasValue;

    public static string ToIdiomatic(this DateOnly date) =>
        date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);

    public static DateTime ToDateTime(this DateOnly date) =>
        date.ToDateTime(TimeOnly.MinValue);

    public static DateTime? ToDateTime(this DateOnly? date) =>
        date?.ToDateTime();

    public static TimeSpan Difference(this DateOnly input, DateOnly other) =>
        (input.ToDateTime() - other.ToDateTime()).Duration();
}