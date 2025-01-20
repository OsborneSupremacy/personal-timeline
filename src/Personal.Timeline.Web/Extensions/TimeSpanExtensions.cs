namespace Personal.Timeline.Web.Extensions;

public static class TimeSpanExtensions
{
    public static string ToIdiomatic(this TimeSpan timeSpan)
    {
        const int daysInYear = 365;
        const int daysInMonth = 30; // Approximation

        var totalDays = timeSpan.TotalDays;

        return totalDays switch
        {
            >= daysInYear => $"{totalDays / daysInYear:F1} year(s)",
            >= daysInMonth => $"{totalDays / daysInMonth:F1} month(s)",
            _ => $"{(int)totalDays} day(s)"
        };
    }
}