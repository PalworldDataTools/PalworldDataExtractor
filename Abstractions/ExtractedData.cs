using System.Text.Json;
using System.Text.Json.Serialization;
using PalworldDataExtractor.Abstractions.Breeding;
using PalworldDataExtractor.Abstractions.Pals;
using PalworldDataExtractor.Abstractions.Steam;

namespace PalworldDataExtractor.Abstractions;

public class ExtractedData
{
    static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

    public required SteamManifest? SteamManifest { get; init; }
    public required IReadOnlyCollection<PalTribe> Tribes { get; init; }
    public required IReadOnlyDictionary<string, byte[]> TribeIcons { get; init; }
    public required IReadOnlyCollection<PalBreedingCombination> UniqueBreedingCombinations { get; init; }

    public static async Task Serialize(ExtractedData data, Stream outStream) =>
        await JsonSerializer.SerializeAsync(outStream, data, typeof(ExtractedData), ExtractedDataJsonSerializerContext.Default);

    public static async Task<ExtractedData?> Deserialize(Stream stream) =>
        await JsonSerializer.DeserializeAsync(stream, typeof(ExtractedData), ExtractedDataJsonSerializerContext.Default) as ExtractedData;
}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(ExtractedData))]
partial class ExtractedDataJsonSerializerContext : JsonSerializerContext
{
}
