using PalworldDataExtractor.Models.Pals;

namespace PalworldDataExtractor.Cli;

public static class Helpers
{
    public static string GetIconFileName(this PalTribe tribe) => tribe.Name + ".png";
    public static string GetDirectoryName(this PalTribe tribe) => tribe.Name;
    public static string GetPalFileName(this Pal pal) => pal.Name + ".json";
}
