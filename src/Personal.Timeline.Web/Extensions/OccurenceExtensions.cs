namespace Personal.Timeline.Web.Extensions;

internal static class OccurenceExtensions
{
    private static readonly Func<Occurrence, string> PastMomentDescriber = occurence =>
    {
        var started = DateOnlyExtensions.Today.Difference(occurence.StartDate);
        return $"{started.ToIdiomatic()} ago";
    };

    private static readonly Func<Occurrence, string> TodayMomentDescriber = _ => "Today";

    private static readonly Func<Occurrence, string> FutureMomentDescriber = occurence =>
    {
        var startsIn = occurence.StartDate.Difference(DateOnlyExtensions.Today);
        return $"{startsIn.ToIdiomatic()} from now";
    };

    private static readonly Func<Occurrence, string> PastToPastEventDescriber = occurence =>
    {
        var started = DateOnlyExtensions.Today.Difference(occurence.StartDate);
        var ended = DateOnlyExtensions.Today.Difference(occurence.EndDate!.Value);
        return $"{started.ToIdiomatic()} to {ended.ToIdiomatic()} ago";
    };

    private static readonly Func<Occurrence, string> PastToTodayEventDescriber = occurence =>
    {
        var started = DateOnlyExtensions.Today.Difference(occurence.StartDate);
        return $"{started.ToIdiomatic()} ago to today";
    };

    private static readonly Func<Occurrence, string> PastToFutureEventDescriber = occurence =>
    {
        var started = DateOnlyExtensions.Today.Difference(occurence.StartDate);
        var endsIn = occurence.EndDate!.Value.Difference(DateOnlyExtensions.Today);
        return $"{started.ToIdiomatic()} to {endsIn.ToIdiomatic()} from now";
    };

    private static readonly Func<Occurrence, string> PastToIndefiniteEventDescriber = occurence =>
    {
        var started = DateOnlyExtensions.Today.Difference(occurence.StartDate);
        return $"{started.ToIdiomatic()} ago and ongoing";
    };

    private static readonly Func<Occurrence, string> TodayToFutureEventDescriber = occurence =>
    {
        var endsIn = occurence.EndDate!.Value.Difference(DateOnlyExtensions.Today);
        return $"Today to {endsIn.ToIdiomatic()} from now";
    };

    private static readonly Func<Occurrence, string> TodayToIndefiniteEventDescriber = _ => "Today and ongoing";

    private static readonly Func<Occurrence, string> FutureToFutureEventDescriber = occurence =>
    {
        var startsIn = occurence.StartDate.Difference(DateOnlyExtensions.Today);
        var endsIn = occurence.EndDate!.Value.Difference(DateOnlyExtensions.Today);
        return $"{startsIn.ToIdiomatic()} to {endsIn.ToIdiomatic()} from now";
    };

    private static readonly Func<Occurrence, string> FutureToIndefiniteEventDescriber = occurence =>
    {
        var startsIn = occurence.StartDate.Difference(DateOnlyExtensions.Today);
        return $"{startsIn.ToIdiomatic()} from now and ongoing";
    };

    private static readonly Dictionary<TemporalStatus, Func<Occurrence, string>> TemporalityDescribers =
        new()
        {
            { TemporalStatus.Past, PastMomentDescriber },
            { TemporalStatus.Today, TodayMomentDescriber },
            { TemporalStatus.Future, FutureMomentDescriber },
            { TemporalStatus.PastToPast, PastToPastEventDescriber },
            { TemporalStatus.PastToToday, PastToTodayEventDescriber },
            { TemporalStatus.PastToFuture, PastToFutureEventDescriber },
            { TemporalStatus.PastToIndefinite, PastToIndefiniteEventDescriber },
            { TemporalStatus.TodayToFuture, TodayToFutureEventDescriber },
            { TemporalStatus.TodayToIndefinite, TodayToIndefiniteEventDescriber },
            { TemporalStatus.FutureToFuture, FutureToFutureEventDescriber },
            { TemporalStatus.FutureToIndefinite, FutureToIndefiniteEventDescriber }
        };

    public static OccurrenceType GetOccurenceType(this Occurrence occurrence) =>
        occurrence.EndDate switch
        {
            null => OccurrenceType.Event,
            _ when occurrence.StartDate.Equals(occurrence.EndDate!.Value) => OccurrenceType.Moment,
            _ => OccurrenceType.Event
        };

    private static TemporalStatus GetTemporalStatus(this Occurrence occurrence)
    {
        var occurenceType = occurrence.GetOccurenceType();
        return occurenceType switch
        {
            OccurrenceType.Moment => occurrence.StartDate switch
            {
                _ when occurrence.StartDate.IsPast() => TemporalStatus.Past,
                _ when occurrence.StartDate.IsToday() => TemporalStatus.Today,
                _ => TemporalStatus.Future
            },
            _ => occurrence.EndDate switch
            {
                _ when occurrence.EndDate.IsPast() => TemporalStatus.PastToPast,
                _ when occurrence.StartDate.IsPast() && occurrence.EndDate.IsToday() => TemporalStatus.PastToToday,
                _ when occurrence.StartDate.IsPast() && occurrence.EndDate.IsFuture() => TemporalStatus.PastToFuture,
                _ when occurrence.StartDate.IsPast() && occurrence.EndDate.IsIndefinite() => TemporalStatus.PastToIndefinite,

                _ when occurrence.StartDate.IsToday() && occurrence.EndDate.IsFuture() => TemporalStatus.TodayToFuture,
                _ when occurrence.StartDate.IsToday() && occurrence.EndDate.IsIndefinite() => TemporalStatus.TodayToIndefinite,

                _ when occurrence.StartDate.IsFuture() && occurrence.EndDate.IsFuture() => TemporalStatus.FutureToFuture,
                _ when occurrence.StartDate.IsFuture() && occurrence.EndDate.IsIndefinite() => TemporalStatus.FutureToIndefinite,

                _ => throw new ArgumentOutOfRangeException($"Unexpected date combination. Start: {occurrence.StartDate}, End: {occurrence.EndDate}")
            }
        };
    }

    public static IEnumerable<string> DescribeTemporality(this Occurrence occurrence) =>
        occurrence.GetOccurenceType() switch
        {
            OccurrenceType.Moment => occurrence.DescribeMomentTemporality(),
            _ => occurrence.DescribeEventTemporality()
        };

    private static IEnumerable<string> DescribeMomentTemporality(this Occurrence occurrence)
    {
        yield return occurrence.StartDate.ToIdiomatic();
        yield return TemporalityDescribers[occurrence.GetTemporalStatus()](occurrence);
    }

    private static IEnumerable<string> DescribeEventTemporality(this Occurrence occurrence) =>
        occurrence.EndDate switch
        {
            null => occurrence.DescribeOngoingEventTemporality(),
            _ => occurrence.DescribeCompletedEventTemporality()
        };

    private static IEnumerable<string> DescribeCompletedEventTemporality(this Occurrence occurrence)
    {
        yield return $"{occurrence.StartDate.ToIdiomatic()} to {occurrence.EndDate!.Value.ToIdiomatic()}";
        yield return occurrence.StartDate
            .Difference(occurrence.EndDate!.Value)
            .ToIdiomatic();
        yield return TemporalityDescribers[occurrence.GetTemporalStatus()](occurrence);
    }

    private static IEnumerable<string> DescribeOngoingEventTemporality(this Occurrence occurrence)
    {
        yield return $"{occurrence.StartDate.ToIdiomatic()} to present";
        yield return occurrence.StartDate
            .Difference(DateOnlyExtensions.Today)
            .ToIdiomatic();
        yield return TemporalityDescribers[occurrence.GetTemporalStatus()](occurrence);
    }
}