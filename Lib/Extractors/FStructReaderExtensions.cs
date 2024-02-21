namespace PalworldDataExtractor.Extractors;

public static class FStructReaderExtensions
{
    public static string? ParseTribeName(this FStructReader reader, string property) => reader.ParseEnumValue(property, "EPalTribeID::");
    public static string? ParseSize(this FStructReader reader, string property) => reader.ParseEnumValue(property, "EPalSizeType::");
    public static string? ParseElementType(this FStructReader reader, string property) => reader.ParseEnumValue(property, "EPalElementType::");
}
