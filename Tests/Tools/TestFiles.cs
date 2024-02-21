namespace Tests.Tools;

public static class TestFiles
{
    const string TestFilesDirectory = "../../../TestFiles";
    public static string GetTestFilePath(string relativePath) => Path.Combine(TestFilesDirectory, relativePath);
}
