using System.Text.Json;
using CommandLine;
using CommandLine.Text;
using CUE4Parse.UE4.Versions;
using PalworldDataExtractor;
using PalworldDataExtractor.Cli;
using PalworldDataExtractor.Models;

ParserResult<Options>? parserResult = new Parser(with => with.HelpWriter = null).ParseArguments<Options>(args);
if (parserResult == null)
{
    DisplayHelp(null);
    return;
}

if (parserResult.Errors.Any())
{
    DisplayHelp(parserResult);
    return;
}

JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true };

Options options = parserResult.Value;
bool quiet = options.Quiet == true;

if (!quiet)
{
    string optionsJson = JsonSerializer.Serialize(options, jsonSerializerOptions);
    Console.WriteLine($"Configuration: {optionsJson}");
    Console.WriteLine();
}

if (!quiet)
{
    Console.WriteLine("Extracting...");
}

DataExtractor extractor = new(
    options.PakFileDirectory,
    config =>
    {
        config.UnrealEngineVersion = ParseVersion(options.UnrealEngineVersion) ?? config.UnrealEngineVersion;
        config.PakFileName = options.PakFileName ?? config.PakFileName;
        config.MappingsFilePath = options.MappingsFilePath ?? config.MappingsFilePath;
    }
);
ExtractedData data = await extractor.Extract();

if (!quiet)
{
    Console.WriteLine("Extraction complete.");
    Console.WriteLine();
}

if (!quiet && data.SteamManifest != null)
{
    string steamManifestJson = JsonSerializer.Serialize(data.SteamManifest, jsonSerializerOptions);
    Console.WriteLine($"Steam app detected: {steamManifestJson}");
    Console.WriteLine();
}

string outputDirectory = Path.GetFullPath(options.OutputDirectory ?? ".");

if (Directory.Exists(outputDirectory))
{
    Directory.Delete(outputDirectory, true);
}
Directory.CreateDirectory(outputDirectory);

string rawDataJsonPath = Path.Combine(outputDirectory, "data.json");

if (!quiet)
{
    Console.WriteLine($"Exporting serialized data to {rawDataJsonPath}...");
}

await using (FileStream fileStream = File.Open(rawDataJsonPath, FileMode.Create))
{
    await ExtractedData.Serialize(data, fileStream);
}

if (!quiet)
{
    Console.WriteLine("Export complete.");
    Console.WriteLine();
}

if (!quiet)
{
    Console.WriteLine($"Exporting to {outputDirectory}...");
}

DataExporter exporter = new(outputDirectory);
await exporter.Export(data);

if (!quiet)
{
    Console.WriteLine("Export complete.");
    Console.WriteLine();
}

if (!quiet)
{
    Console.WriteLine("Statistics:");
    Console.WriteLine($"Tribes: {data.Tribes.Count}");
    Console.WriteLine($"Pals: {data.Tribes.Sum(t => t.Pals.Count)}");
    Console.WriteLine();
}

return;

VersionContainer? ParseVersion(string? version)
{
    EGame? gameVersion = version switch
    {
        "4.0" => EGame.GAME_UE4_0,
        "4.1" => EGame.GAME_UE4_1,
        "4.2" => EGame.GAME_UE4_2,
        "4.3" => EGame.GAME_UE4_3,
        "4.4" => EGame.GAME_UE4_4,
        "4.5" => EGame.GAME_UE4_5,
        "4.6" => EGame.GAME_UE4_6,
        "4.7" => EGame.GAME_UE4_7,
        "4.8" => EGame.GAME_UE4_8,
        "4.9" => EGame.GAME_UE4_9,
        "4.10" => EGame.GAME_UE4_10,
        "4.11" => EGame.GAME_UE4_11,
        "4.12" => EGame.GAME_UE4_12,
        "4.13" => EGame.GAME_UE4_13,
        "4.14" => EGame.GAME_UE4_14,
        "4.15" => EGame.GAME_UE4_15,
        "4.16" => EGame.GAME_UE4_16,
        "4.17" => EGame.GAME_UE4_17,
        "4.18" => EGame.GAME_UE4_18,
        "4.19" => EGame.GAME_UE4_19,
        "4.20" => EGame.GAME_UE4_20,
        "4.21" => EGame.GAME_UE4_21,
        "4.22" => EGame.GAME_UE4_22,
        "4.23" => EGame.GAME_UE4_23,
        "4.24" => EGame.GAME_UE4_24,
        "4.25" => EGame.GAME_UE4_25,
        "4.25+" => EGame.GAME_UE4_25_Plus,
        "4.26" => EGame.GAME_UE4_26,
        "4.27" => EGame.GAME_UE4_27,
        "4.28" => EGame.GAME_UE4_28,
        "5.0" => EGame.GAME_UE5_0,
        "5.1" => EGame.GAME_UE5_1,
        "5.2" => EGame.GAME_UE5_2,
        "5.3" => EGame.GAME_UE5_3,
        "5.4" => EGame.GAME_UE5_4,
        "5.5" => EGame.GAME_UE5_5,
        "4" => EGame.GAME_UE4_LATEST,
        "5" => EGame.GAME_UE5_LATEST,
        _ => null
    };

    return gameVersion.HasValue ? new VersionContainer(gameVersion.Value) : null;

}

void DisplayHelp(ParserResult<Options>? result)
{
    const string nameAndVersion = $"Palworld Data Extractor v{Metadata.Version}";

    string helpText;
    if (result != null && result.Errors.IsVersion())
    {
        helpText = nameAndVersion;
    }
    else
    {
        helpText = HelpText.AutoBuild(
            result,
            h =>
            {
                h.AdditionalNewLineAfterOption = false;
                h.AddNewLineBetweenHelpSections = true;
                h.Heading = nameAndVersion;
                h.OptionComparison = HelpText.RequiredThenAlphaComparison;
                return h;
            }
        );
    }

    Console.WriteLine(helpText);
}
