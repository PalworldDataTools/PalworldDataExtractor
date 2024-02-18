using System.Text.Json;
using PalworldDataExtractor.Models.Pals;
using PalworldDataExtractor.Models.Steam;

namespace PalworldDataExtractor.Models;

public class ExtractedData
{
    static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

    public required SteamManifest? SteamManifest { get; init; }
    public required IReadOnlyCollection<PalTribe> Tribes { get; init; }
    public required IReadOnlyDictionary<string, byte[]> TribeIcons { get; init; }

    internal ExtractedData() { }

    public static async Task Serialize(ExtractedData data, Stream outStream) => await JsonSerializer.SerializeAsync(outStream, data, SerializerOptions);
    public static async Task<ExtractedData?> Deserialize(Stream stream) => await JsonSerializer.DeserializeAsync<ExtractedData>(stream, SerializerOptions);
}
