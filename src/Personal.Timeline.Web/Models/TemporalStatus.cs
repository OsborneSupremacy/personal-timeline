namespace Personal.Timeline.Web.Models;

internal enum TemporalStatus
{
    Past,
    PastToPast,
    PastToToday,
    PastToFuture,
    PastToIndefinite,
    Today,
    TodayToFuture,
    TodayToIndefinite,
    Future,
    FutureToFuture,
    FutureToIndefinite
}