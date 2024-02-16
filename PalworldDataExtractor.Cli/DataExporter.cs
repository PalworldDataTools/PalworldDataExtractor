using System.Text.Json;
using System.Text.Json.Serialization;
using PalworldDataExtractor.Models;
using PalworldDataExtractor.Models.Pals;

namespace PalworldDataExtractor.Cli;

public class DataExporter
{
    static readonly JsonSerializerOptions DefaultSerializerOptions = new() { WriteIndented = true, Converters = { new JsonStringEnumConverter() } };
    const string TribesDirectory = "Tribes";

    readonly string _targetDirectory;
    readonly JsonSerializerOptions _jsonSerializerOptions;

    public DataExporter(string targetDirectory) : this(targetDirectory, DefaultSerializerOptions) { }

    public DataExporter(string targetDirectory, JsonSerializerOptions jsonSerializerOptions)
    {
        _targetDirectory = targetDirectory;
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public async Task Export(ExtractedData data)
    {
        if (Directory.Exists(_targetDirectory))
        {
            Directory.Delete(_targetDirectory, true);
        }

        DirectoryInfo root = Directory.CreateDirectory(_targetDirectory);
        DirectoryInfo tribesDirectory = root.CreateSubdirectory(TribesDirectory);

        List<Task> work = new();
        work.AddRange(data.Tribes.Select(tribe => ExportTribe(tribesDirectory, tribe)));

        await Task.WhenAll(work);
    }

    async Task ExportTribe(DirectoryInfo root, PalTribe tribe)
    {
        DirectoryInfo directory = root.CreateSubdirectory(tribe.Name);
        await Task.WhenAll(tribe.Pals.Select(pal => ExportPal(directory, pal)));
    }

    async Task ExportPal(DirectoryInfo root, Pal pal)
    {
        string filePath = Path.Combine(root.FullName, pal.Name + ".json");
        await using FileStream stream = File.OpenWrite(filePath);
        await JsonSerializer.SerializeAsync(stream, pal, _jsonSerializerOptions);
    }
}
