using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using PalworldDataExtractor.Abstractions.Steam;

namespace PalworldDataExtractor.Extractors;

partial class SteamManifestExtractor
{
    readonly string _steamAppsDirectory;

    public SteamManifestExtractor(string steamAppsDirectory)
    {
        _steamAppsDirectory = steamAppsDirectory;
    }

    public async Task<SteamManifest?> Extract()
    {
        IReadOnlyDictionary<string, string>? manifest = await TryGetManifest();
        if (manifest == null)
        {
            return null;
        }

        return new SteamManifest
        {
            AppId = TryGetValue(manifest, "AppId", out string? appId) ? appId : "",
            BuildId = TryGetValue(manifest, "BuildId", out string? buildId) ? buildId : "",
            AppName = TryGetValue(manifest, "Name", out string? appName) ? appName : "",
            AppSize = TryGetValue(manifest, "SizeOnDisk", out string? appSizeStr) && long.TryParse(appSizeStr, out long appSize) ? appSize : 0,
            UpdateDate = TryGetValue(manifest, "LastUpdated", out string? lastUpdatedStr) && long.TryParse(lastUpdatedStr, out long lastUpdatedTimestamp)
                ? DateOnly.FromDateTime(DateTimeOffset.FromUnixTimeSeconds(lastUpdatedTimestamp).Date)
                : null
        };
    }

    async Task<IReadOnlyDictionary<string, string>?> TryGetManifest()
    {
        IEnumerable<string> manifests = Directory.EnumerateFiles(_steamAppsDirectory, "*.acf");
        foreach (string manifestFile in manifests)
        {
            string content = await File.ReadAllTextAsync(manifestFile);
            IReadOnlyDictionary<string, string> manifest = ParseManifest(content);

            if (!TryGetValue(manifest, "name", out string? value) || !value.Contains("palworld", StringComparison.InvariantCultureIgnoreCase))
            {
                continue;
            }

            return manifest;
        }

        return null;
    }

    bool TryGetValue(IReadOnlyDictionary<string, string> manifest, string field, [NotNullWhen(true)] out string? value)
    {
        foreach (KeyValuePair<string, string> entry in manifest)
        {
            if (entry.Key.Equals(field, StringComparison.InvariantCultureIgnoreCase))
            {
                value = entry.Value;
                return true;
            }
        }

        value = null;
        return false;
    }

    IReadOnlyDictionary<string, string> ParseManifest(string content)
    {
        Dictionary<string, string> result = new();

        foreach (string line in content.Split('\n', '\r'))
        {
            Match match = ParseSteamManifestLine().Match(line);
            if (!match.Success)
            {
                continue;
            }

            string field = match.Groups["field"].Value;
            string value = match.Groups["value"].Value;

            result[field] = value;
        }

        return result;
    }

    [GeneratedRegex("\"(?<field>.*)\"[^\"]*\"(?<value>.*)\"")]
    private static partial Regex ParseSteamManifestLine();
}
