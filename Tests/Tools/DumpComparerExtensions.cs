using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;

namespace Tests.Tools;

public static class DumpComparerExtensions
{
    static readonly JsonSerializerOptions JsonSerializerOptions = new() { WriteIndented = true, Converters = { new JsonStringEnumConverter() } };

    public static async Task DumpStreamAndCompare<T>(this T obj, Func<T, Stream, Task> serialize, string dumpFileName, bool overwrite = false)
    {
        string dumpPath = TestFiles.GetTestFilePath(dumpFileName);

        string tmpFileName = Path.GetTempFileName();
        await using (FileStream tmpFileStream = File.OpenWrite(tmpFileName))
        {
            await serialize(obj, tmpFileStream);
        }

        byte[] content = await File.ReadAllBytesAsync(tmpFileName);

        try
        {
            byte[]? expected = File.Exists(dumpPath) ? await File.ReadAllBytesAsync(dumpPath) : null;
            content.Should().BeEquivalentTo(expected);
        }
        finally
        {
            if (overwrite)
            {
                await File.WriteAllBytesAsync(dumpPath, content);
            }
        }
    }

    public static void DumpJsonAndCompare(this object obj, string dumpFileName, bool overwrite = false)
    {
        string dumpPath = TestFiles.GetTestFilePath(dumpFileName);
        string content = JsonSerializer.Serialize(obj, JsonSerializerOptions);

        try
        {
            string? expected = File.Exists(dumpPath) ? File.ReadAllText(dumpPath) : null;
            content.Should().Be(expected);
        }
        finally
        {
            if (overwrite)
            {
                File.WriteAllText(dumpPath, content);
            }
        }
    }
}
