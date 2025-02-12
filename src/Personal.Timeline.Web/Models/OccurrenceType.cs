namespace Personal.Timeline.Web.Models;

public enum OccurrenceType
{
    /// <summary>
    /// An occurrence that can be represented as a single point in time.
    /// </summary>
    Moment,
    /// <summary>
    /// An occurrence that can be represented as a range of time.
    /// </summary>
    Event
}