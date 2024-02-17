﻿namespace PalworldDataExtractor.Models.Steam;

public class SteamManifest
{
    public required string AppId { get; init; }
    public required string BuildId { get; init; }
    public required string AppName { get; init; }
    public required long AppSize { get; init; }

    internal SteamManifest() { }
}