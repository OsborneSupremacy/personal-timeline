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
            >= daysInYear => $"{totalDays / daysInYear:F1} year{(totalDays / daysInYear > 1 ? "s" : string.Empty)}",
            >= daysInMonth => $"{totalDays / daysInMonth:F1} month{(totalDays / daysInMonth > 1 ? "s" : string.Empty)}",
            _ => $"{totalDays} day{(totalDays > 1 ? "s" : "")}"
        };
    }
}