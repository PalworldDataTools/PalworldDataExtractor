using CUE4Parse.UE4.Versions;

namespace PalworldDataExtractor;

public class DataExtractorConfiguration
{
    public VersionContainer UnrealEngineVersion { get; set; } = VersionContainer.DEFAULT_VERSION_CONTAINER;
    public string PakFileName { get; set; } = "Pal-Windows.pak";
    public string? MappingsFilePath { get; set; } = null;
}
