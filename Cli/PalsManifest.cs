using PalworldDataExtractor.Models;
using PalworldDataExtractor.Models.Pals;

namespace PalworldDataExtractor.Cli;

public static class PalsManifest
{
    public static IReadOnlyDictionary<string, Entry> FromExtractedData(ExtractedData data) => data.Tribes.ToDictionary(t => t.Name, t => ComputeEntry(t));

    static Entry ComputeEntry(PalTribe tribe)
    {
        if (tribe.Pals.Count == 0)
        {
            throw new InvalidOperationException("Tribe was not expected to be empty");
        }

        string directoryName = tribe.GetDirectoryName();
        Pal mainPal = tribe.Pals.FirstOrDefault(p => p is { IsBoss: false, IsTowerBoss: false }) ?? tribe.Pals.First();
        Pal? bossPal = tribe.Pals.FirstOrDefault(p => p is { IsBoss: true, IsTowerBoss: false });
        Pal? gymPal = tribe.Pals.FirstOrDefault(p => p is { IsTowerBoss: true });
        Pal[] others = tribe.Pals.Where(p => p != mainPal && p != bossPal && p != gymPal).ToArray();

        return new Entry
        {
            Icon = Path.Combine(directoryName, tribe.GetIconFileName()),
            Main = Path.Combine(directoryName, mainPal.GetPalFileName()),
            Boss = bossPal != null ? Path.Combine(directoryName, bossPal.GetPalFileName()) : null,
            Gym = gymPal != null ? Path.Combine(directoryName, gymPal.GetPalFileName()) : null,
            OtherVariants = others.Length > 0 ? others.Select(p => Path.Combine(directoryName, p.GetPalFileName())).ToArray() : null
        };
    }

    public class Entry
    {
        public required string Icon { get; init; }
        public required string Main { get; init; }
        public required string? Boss { get; init; }
        public required string? Gym { get; init; }
        public required IReadOnlyCollection<string>? OtherVariants { get; init; }
    }
}
