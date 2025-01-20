namespace Personal.Timeline.Web.Utility;

internal static class ReflectionUtilities
{
    public static string GetBaseOutputPath() =>
        Path.Combine(
            new FileInfo(
                System.Reflection.Assembly.GetExecutingAssembly().Location
            ).DirectoryName!, 
            "output"
        );
}