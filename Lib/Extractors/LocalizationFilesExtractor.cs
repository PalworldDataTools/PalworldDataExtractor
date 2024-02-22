using CUE4Parse.FileProvider;
using CUE4Parse.UE4.Assets.Exports.Engine;
using CUE4Parse.Utils;
using PalworldDataExtractor.Abstractions.L10N;
using PalworldDataExtractor.Readers;

namespace PalworldDataExtractor.Extractors;

public class LocalizationFilesExtractor
{
    readonly DefaultFileProvider _provider;
    readonly UDataTableReader _tableReader;

    public LocalizationFilesExtractor(DefaultFileProvider provider)
    {
        _provider = provider;
        _tableReader = new UDataTableReader(provider);
    }

    public async Task<Dictionary<string, LocalizationFile>> ExtractLocalizationFilesAsync()
    {
        const string l10NFilesRoot = "Pal/Content/L10N/";
        string[] l10NFiles = _provider.Files.Where(kv => kv.Key.StartsWith(l10NFilesRoot) && kv.Key.EndsWith(".uasset"))
            .Select(kv => kv.Key.SubstringAfter(l10NFilesRoot).SubstringBeforeLast(".uasset"))
            .ToArray();
        IEnumerable<IGrouping<string, string>> l10NFilesByRegion = l10NFiles.GroupBy(f => f.SubstringBefore("/"));

        Dictionary<string, LocalizationFile> dictionary = new();
        foreach (IGrouping<string, string> group in l10NFilesByRegion)
        {
            LocalizationFile file = await ExtractLocalizationFileAsync(group.Key, group.Select(p => l10NFilesRoot + p).ToArray());
            dictionary.Add(group.Key, file);
        }

        return dictionary;
    }

    async Task<LocalizationFile> ExtractLocalizationFileAsync(string language, IEnumerable<string> namespaceFilePaths)
    {
        Dictionary<string, LocalizationNamespace> namespaces = new();

        foreach (string path in namespaceFilePaths)
        {
            LocalizationNamespace ns = await ExtractLocalizationNamespace(path);
            namespaces.Add(ns.Namespace, ns);
        }

        return new LocalizationFile { Language = language, Namespaces = namespaces };
    }

    async Task<LocalizationNamespace> ExtractLocalizationNamespace(string path)
    {
        UDataTable table = await _tableReader.ExtractAsync(path);

        return new LocalizationNamespace
        {
            Namespace = table.Name,
            Fields = table.RowMap.ToDictionary(
                kv => kv.Key.Text,
                kv =>
                {
                    FStructReader reader = new(kv.Value);
                    return reader.ParseString("TextData") ?? kv.Key.Text;
                }
            )
        };
    }
}
