namespace Personal.Timeline.Web.Utility;

internal static class ContentUtilities
{
    public static async Task<string> ReadAllTextAsync(params string[] paths)
    {
        var pathsWithBase = new string[paths.Length + 1];
        pathsWithBase[0] = ReflectionUtilities.GetBaseOutputPath();
        paths.CopyTo(pathsWithBase, 1);
        return await File.ReadAllTextAsync(
            Path.Combine(pathsWithBase)
        );
    }
}