using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;

namespace Tests.Tools;

public static class DumpComparerExtensions
{
    const string DumpFileDirectory = "../../../";
    static readonly JsonSerializerOptions JsonSerializerOptions = new() { WriteIndented = true, Converters = { new JsonStringEnumConverter() } };

    public static void DumpCompare(this object obj, string dumpFileName, bool overwrite = false)
    {
        string dumpPath = Path.Combine(DumpFileDirectory, dumpFileName);
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
