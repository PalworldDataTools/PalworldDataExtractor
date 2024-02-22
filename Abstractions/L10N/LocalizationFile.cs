namespace PalworldDataExtractor.Abstractions.L10N;

public class LocalizationFile
{
    public required string Language { get; set; }
    public IReadOnlyDictionary<string, LocalizationNamespace> Namespaces { get; set; } = new Dictionary<string, LocalizationNamespace>();
}

public class LocalizationNamespace
{
    public required string Namespace { get; set; }
    public IReadOnlyDictionary<string, string> Fields { get; set; } = new Dictionary<string, string>();
}
