﻿using System.Text.Json;
using System.Text.Json.Serialization;
using PalworldDataExtractor.Models.Pals;
using PalworldDataExtractor.Models.Steam;

namespace PalworldDataExtractor.Models;

public class ExtractedData
{
    static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

    public required SteamManifest? SteamManifest { get; init; }
    public required IReadOnlyCollection<PalTribe> Tribes { get; init; }
    public required IReadOnlyDictionary<string, byte[]> TribeIcons { get; init; }

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
