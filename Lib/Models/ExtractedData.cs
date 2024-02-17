using PalworldDataExtractor.Models.Pals;
using PalworldDataExtractor.Models.Steam;

namespace PalworldDataExtractor.Models;

public class ExtractedData
{
    public required SteamManifest? SteamManifest { get; init; }
    public required IReadOnlyCollection<PalTribe> Tribes { get; init; }
    public required IReadOnlyDictionary<string, byte[]> TribeIcons { get; init; }

    internal ExtractedData() { }
}
