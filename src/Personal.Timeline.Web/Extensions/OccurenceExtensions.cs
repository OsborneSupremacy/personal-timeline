namespace Personal.Timeline.Web.Extensions;

internal static class OccurenceExtensions
{
    private static readonly Func<Occurence, string> PastMomentDescriber = occurence =>
    {
        var started = DateOnlyExtensions.Today.Difference(occurence.StartDate);
        return $"{started.ToIdiomatic()} ago";
    };

    private static readonly Func<Occurence, string> TodayMomentDescriber = occurence => "Today";

    private static readonly Func<Occurence, string> FutureMomentDescriber = occurence =>
    {
        var startsIn = occurence.StartDate.Difference(DateOnlyExtensions.Today);
        return $"{startsIn.ToIdiomatic()} from now";
    };

    private static readonly Func<Occurence, string> PastToPastEventDescriber = occurence =>
    {
        var started = DateOnlyExtensions.Today.Difference(occurence.StartDate);
        var ended = DateOnlyExtensions.Today.Difference(occurence.EndDate!.Value);
        return $"{started.ToIdiomatic()} to {ended.ToIdiomatic()} ago";
    };

    private static readonly Func<Occurence, string> PastToTodayEventDescriber = occurence =>
    {
        var started = DateOnlyExtensions.Today.Difference(occurence.StartDate);
        return $"{started.ToIdiomatic()} ago to today";
    };

    private static readonly Func<Occurence, string> PastToFutureEventDescriber = occurence =>
    {
        var started = DateOnlyExtensions.Today.Difference(occurence.StartDate);
        var endsIn = occurence.EndDate!.Value.Difference(DateOnlyExtensions.Today);
        return $"{started.ToIdiomatic()} to {endsIn.ToIdiomatic()} from now";
    };

    private static readonly Func<Occurence, string> PastToIndefiniteEventDescriber = occurence =>
    {
        var started = DateOnlyExtensions.Today.Difference(occurence.StartDate);
        return $"{started.ToIdiomatic()} ago and ongoing";
    };

    private static readonly Func<Occurence, string> TodayToFutureEventDescriber = occurence =>
    {
        var endsIn = occurence.EndDate!.Value.Difference(DateOnlyExtensions.Today);
        return $"Today to {endsIn.ToIdiomatic()} from now";
    };

    private static readonly Func<Occurence, string> TodayToIndefiniteEventDescriber = occurence => "Today and ongoing";

    private static readonly Func<Occurence, string> FutureToFutureEventDescriber = occurence =>
    {
        var startsIn = occurence.StartDate.Difference(DateOnlyExtensions.Today);
        var endsIn = occurence.EndDate!.Value.Difference(DateOnlyExtensions.Today);
        return $"{startsIn.ToIdiomatic()} to {endsIn.ToIdiomatic()} from now";
    };

    private static readonly Func<Occurence, string> FutureToIndefiniteEventDescriber = occurence =>
    {
        var startsIn = occurence.StartDate.Difference(DateOnlyExtensions.Today);
        return $"{startsIn.ToIdiomatic()} from now and ongoing";
    };

    private static readonly Dictionary<TemporalStatus, Func<Occurence, string>> TemporalityDescribers =
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

    public static OccurenceType GetOccurenceType(this Occurence occurence) =>
        occurence.EndDate switch
        {
            null => OccurenceType.Event,
            _ when occurence.StartDate.Equals(occurence.EndDate!.Value) => OccurenceType.Moment,
            _ => OccurenceType.Event
        };

    private static TemporalStatus GetTemporalStatus(this Occurence occurence)
    {
        var occurenceType = occurence.GetOccurenceType();
        return occurenceType switch
        {
            OccurenceType.Moment => occurence.StartDate switch
            {
                _ when occurence.StartDate.IsPast() => TemporalStatus.Past,
                _ when occurence.StartDate.IsToday() => TemporalStatus.Today,
                _ => TemporalStatus.Future
            },
            _ => occurence.EndDate switch
            {
                _ when occurence.EndDate.IsPast() => TemporalStatus.PastToPast,
                _ when occurence.StartDate.IsPast() && occurence.EndDate.IsToday() => TemporalStatus.PastToToday,
                _ when occurence.StartDate.IsPast() && occurence.EndDate.IsFuture() => TemporalStatus.PastToFuture,
                _ when occurence.StartDate.IsPast() && occurence.EndDate.IsIndefinite() => TemporalStatus.PastToIndefinite,

                _ when occurence.StartDate.IsToday() && occurence.EndDate.IsFuture() => TemporalStatus.TodayToFuture,
                _ when occurence.StartDate.IsToday() && occurence.EndDate.IsIndefinite() => TemporalStatus.TodayToIndefinite,

                _ when occurence.StartDate.IsFuture() && occurence.EndDate.IsFuture() => TemporalStatus.FutureToFuture,
                _ when occurence.StartDate.IsFuture() && occurence.EndDate.IsIndefinite() => TemporalStatus.FutureToIndefinite,

                _ => throw new ArgumentOutOfRangeException($"Unexpected date combination. Start: {occurence.StartDate}, End: {occurence.EndDate}")
            }
        };
    }

    public static IEnumerable<string> DescribeTemporality(this Occurence occurence)
    {
        var occurenceType = occurence.GetOccurenceType();
        return occurenceType == OccurenceType.Moment ?
            occurence.DescribeMomentTemporality() :
            occurence.DescribeEventTemporality();
    }

    private static IEnumerable<string> DescribeMomentTemporality(this Occurence occurence)
    {
        yield return occurence.StartDate.ToIdiomatic();
        yield return TemporalityDescribers[occurence.GetTemporalStatus()](occurence);
    }

    private static IEnumerable<string> DescribeEventTemporality(this Occurence occurence)
    {
        yield return occurence.EndDate switch
        {
            not null => $"{occurence.StartDate.ToIdiomatic()} to {occurence.EndDate.Value.ToIdiomatic()}",
            _ => $"{occurence.StartDate.ToIdiomatic()} and ongoing"
        };

        yield return occurence.StartDate.Difference(occurence.EndDate ?? DateOnlyExtensions.Today)
            .ToIdiomatic();

        yield return TemporalityDescribers[occurence.GetTemporalStatus()](occurence);
    }
}