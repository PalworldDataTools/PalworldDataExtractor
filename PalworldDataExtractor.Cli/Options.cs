using CommandLine;

namespace PalworldDataExtractor.Cli;

class Options
{
    const string DefaultPakFileDirectory = ".";
    const string DefaultPakFileName = "Pal-Windows.pak";
    const string DefaultUnrealEngineVersion = "5.1";
    const string DefaultMappingsFilePath = "mappings.usmap";
    const string DefaultOutputDirectory = "Export";

    [Value(0, MetaName = "pak_directory", HelpText = $".pak file directory (default: {DefaultPakFileDirectory})")]
    public string PakFileDirectory { get; set; } = DefaultPakFileDirectory;

    [Option('o', "out", HelpText = $"output directory (default: {DefaultOutputDirectory})")]
    public string OutputDirectory { get; set; } = DefaultOutputDirectory;

    [Option('p', "pak", HelpText = $".pak file name (default: {DefaultPakFileName})")]
    public string PakFileName { get; set; } = DefaultPakFileName;

    [Option("ue-version", HelpText = $"Version of UnrealEngine to use (default: {DefaultUnrealEngineVersion})")]
    public string UnrealEngineVersion { get; set; } = DefaultUnrealEngineVersion;

    [Option("usmap", HelpText = $".pak file name (default: {DefaultMappingsFilePath})")]
    public string MappingsFilePath { get; set; } = DefaultMappingsFilePath;

    [Option('q', "quiet", HelpText = "do not print anything else than errors (default: false)")]
    public bool Quiet { get; set; } = false;
}
