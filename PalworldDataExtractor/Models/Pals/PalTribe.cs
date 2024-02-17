namespace PalworldDataExtractor.Models.Pals;

public class PalTribe
{
    public required string Name { get; init; }
    public required IReadOnlyCollection<Pal> Pals { get; init; }
}
