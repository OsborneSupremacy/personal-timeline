namespace Personal.Timeline.Web.Models.Js3;

internal record Js3Title
{
    public Js3Media? Media { get; init; }
    
    public Js3Text? Text { get; init; }
}