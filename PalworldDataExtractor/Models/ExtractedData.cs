using PalworldDataExtractor.Models.Pals;

namespace PalworldDataExtractor.Models;

public class ExtractedData
{
    public required IReadOnlyCollection<PalTribe> Tribes { get; init; }
}
