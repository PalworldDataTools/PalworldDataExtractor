using System.Text.Json;
using System.Text.Json.Serialization;
using PalworldDataExtractor.Models;
using PalworldDataExtractor.Models.Pals;

namespace PalworldDataExtractor.Cli;

public class DataExporter
{
    static readonly JsonSerializerOptions DefaultSerializerOptions = new()
        { WriteIndented = true, Converters = { new JsonStringEnumConverter() }, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
    const string TribesDirectory = "Pals";
    const string EnumsDirectory = "Enums";
    const string PalsManifestFileName = "pals";
    const string SteamManifestFileName = "steam";

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


        work.Add(ExportSteamManifest(root, data));
        work.AddRange(data.Tribes.Select(tribe => ExportTribe(tribesDirectory, tribe, data.TribeIcons.GetValueOrDefault(tribe.Name))));
        work.Add(ExportPalsManifest(tribesDirectory, data));

        await Task.WhenAll(work);
    }

    async Task ExportSteamManifest(DirectoryInfo root, ExtractedData data)
    {
        if (data.SteamManifest == null)
        {
            return;
        }

        string filePath = Path.Combine(root.FullName, SteamManifestFileName + ".json");
        await WriteAsJson(data.SteamManifest, filePath);
    }

    async Task ExportTribe(DirectoryInfo root, PalTribe tribe, byte[]? icon)
    {
        DirectoryInfo directory = root.CreateSubdirectory(tribe.GetDirectoryName());

        List<Task> work = new();

        work.AddRange(tribe.Pals.Select(pal => ExportPal(directory, pal)));

        if (icon != null)
        {
            work.Add(ExportIcon(directory, tribe, icon));
        }

        await Task.WhenAll(work);
    }

    async Task ExportPalsManifest(DirectoryInfo root, ExtractedData data)
    {
        string filePath = Path.Combine(root.FullName, PalsManifestFileName + ".json");
        IReadOnlyDictionary<string, PalsManifest.Entry> manifest = PalsManifest.FromExtractedData(data);
        await WriteAsJson(manifest, filePath);
    }

    async Task ExportPal(DirectoryInfo root, Pal pal)
    {
        string filePath = Path.Combine(root.FullName, pal.GetPalFileName());
        await WriteAsJson(pal, filePath);
    }

    static async Task ExportIcon(DirectoryInfo root, PalTribe tribe, byte[] icon)
    {
        string filePath = Path.Combine(root.FullName, tribe.GetIconFileName());
        await File.WriteAllBytesAsync(filePath, icon);
    }

    async Task ExportEnum(DirectoryInfo root, string enumName, Func<Pal, IEnumerable<string>> getValues, ExtractedData data)
    {
        string fileName = enumName + ".json";
        string filePath = Path.Combine(root.FullName, fileName);

        string[] values = data.Tribes.SelectMany(t => t.Pals).SelectMany(getValues).Distinct().Order().ToArray();

        await WriteAsJson(values, filePath);
    }

    async Task WriteAsJson(object obj, string path)
    {
        await using FileStream stream = File.OpenWrite(path);
        await JsonSerializer.SerializeAsync(stream, obj, _jsonSerializerOptions);
    }
}
