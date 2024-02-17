using System.Text.Json;
using System.Text.Json.Serialization;
using PalworldDataExtractor.Models;
using PalworldDataExtractor.Models.Pals;

namespace PalworldDataExtractor.Cli;

public class DataExporter
{
    static readonly JsonSerializerOptions DefaultSerializerOptions = new() { WriteIndented = true, Converters = { new JsonStringEnumConverter() } };
    const string TribesDirectory = "PalTribes";
    const string EnumsDirectory = "Enums";

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
        DirectoryInfo enumsDirectory = root.CreateSubdirectory(EnumsDirectory);
        DirectoryInfo tribesDirectory = root.CreateSubdirectory(TribesDirectory);

        List<Task> work =
        [
            ExportEnum(enumsDirectory, "ElementType", p => new[] { p.ElementType1, p.ElementType2 }, data),
            ExportEnum(enumsDirectory, "GenusCategoryType", p => new[] { p.GenusCategory }, data),
            ExportEnum(enumsDirectory, "OrganizationType", p => new[] { p.OrganizationType }, data),
            ExportEnum(enumsDirectory, "SizeType", p => new[] { p.Size }, data),
            ExportEnum(enumsDirectory, "WeaponType", p => new[] { p.WeaponType }, data)
        ];

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

    async Task ExportEnum(DirectoryInfo root, string enumName, Func<Pal, IEnumerable<string>> getValues, ExtractedData data)
    {
        string fileName = enumName + ".json";
        string filePath = Path.Combine(root.FullName, fileName);

        string[] values = data.Tribes.SelectMany(t => t.Pals).SelectMany(getValues).Distinct().Order().ToArray();

        await using FileStream stream = File.OpenWrite(filePath);
        await JsonSerializer.SerializeAsync(stream, values, _jsonSerializerOptions);
    }
}
