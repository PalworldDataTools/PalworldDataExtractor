using CommandLine;
using CommandLine.Text;

namespace PalworldDataExtractor.Cli;

class Options
{
    const string DefaultPakFileName = "Pal-Windows.pak";
    const string DefaultUnrealEngineVersion = "5.1";
    const string DefaultMappingsFilePath = "mappings.usmap";
    const string DefaultOutputDirectory = "Export";

    [Value(0, Required = true, MetaName = "dir", HelpText = ".pak file directory")]
    public string PakFileDirectory { get; set; } = "";

    [Option('o', "out", HelpText = "Output directory", Default = DefaultOutputDirectory)]
    public string? OutputDirectory { get; set; }

    [Option('p', "pak", HelpText = ".pak file name", Default = DefaultPakFileName)]
    public string? PakFileName { get; set; }

    [Option("ue-version", HelpText = "Version of UnrealEngine to use", Default = DefaultUnrealEngineVersion)]
    public string? UnrealEngineVersion { get; set; }

    [Option("usmap", HelpText = ".usmap mapping file to use", Default = DefaultMappingsFilePath)]
    public string? MappingsFilePath { get; set; }

    [Option('q', "quiet", HelpText = "Do not print anything else than errors", Default = false)]
    public bool? Quiet { get; set; }

    [Usage(ApplicationAlias = "PalworldDataExtractor.exe")]
    public static IEnumerable<Example> Examples {
        get {
            yield return new Example(
                "minimal",
                new Options
                {
                    PakFileDirectory = @"Palworld\Pal\Content\Paks",
                    OutputDirectory = null,
                    PakFileName = null,
                    UnrealEngineVersion = null,
                    MappingsFilePath = null
                }
            );
        }
    }
}
