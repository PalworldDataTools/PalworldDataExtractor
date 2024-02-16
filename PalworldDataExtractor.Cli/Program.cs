using System.Text.Json;
using CommandLine;
using CommandLine.Text;
using CUE4Parse.UE4.Versions;
using PalworldDataExtractor;
using PalworldDataExtractor.Cli;
using PalworldDataExtractor.Models;

ParserResult<Options>? parserResult = new Parser().ParseArguments<Options>(args);
if (parserResult == null)
{
    DisplayHelp(null);
    return;
}

if (parserResult.Errors.Any())
{
    DisplayErrors(parserResult);
    return;
}

JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true };

Options options = parserResult.Value;
string optionsJson = JsonSerializer.Serialize(options, jsonSerializerOptions);

if (!options.Quiet)
{
    Console.WriteLine($"Configuration: {optionsJson}");
    Console.WriteLine();
}

if (!options.Quiet)
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
ExtractedData result = await extractor.Extract();

if (!options.Quiet)
{
    Console.WriteLine("Extraction complete.");
    Console.WriteLine();
}

string outputDirectory = Path.GetFullPath(options.OutputDirectory);

if (!options.Quiet)
{
    Console.WriteLine($"Exporting to {outputDirectory}...");
}

DataExporter exporter = new(outputDirectory);
await exporter.Export(result);

if (!options.Quiet)
{
    Console.WriteLine("Export complete.");
    Console.WriteLine();
}

if (!options.Quiet)
{
    Console.WriteLine("Statistics:");
    Console.WriteLine($"Tribes: {result.Tribes.Count}");
    Console.WriteLine($"Pals: {result.Tribes.Sum(t => t.Pals.Count)}");
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
    Console.WriteLine(
        HelpText.AutoBuild(
            result,
            h =>
            {
                h.AdditionalNewLineAfterOption = false;
                h.Heading = "Palworld Data Extractor v0.1";
                return h;
            }
        )
    );
}

void DisplayErrors(ParserResult<Options> result)
{
    foreach (Error? err in result.Errors)
    {
        Console.Error.WriteLine(err);
    }

    Console.Error.WriteLine();

    DisplayHelp(result);
}
