using CUE4Parse.FileProvider;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Exports.Engine;
using PalworldDataExtractor.Models.Pals;
using PalworldDataExtractor.Parsers;

namespace PalworldDataExtractor.Extractors;

class PalsExtractor
{
    readonly DefaultFileProvider _provider;

    public PalsExtractor(DefaultFileProvider provider)
    {
        _provider = provider;
    }

    public async Task<IEnumerable<Pal>> ExtractPalsAsync()
    {
        const string palAssetFilePath = @"Game\Pal\DataTable\Character\DT_PalMonsterParameter";

        List<UObject> palAssets = (await _provider.LoadAllObjectsAsync(palAssetFilePath)).ToList();
        if (palAssets.Count == 0)
        {
            throw new InvalidOperationException($"Could not find pal table in asset {palAssetFilePath}");
        }

        UObject palTableAsset = palAssets.First();
        if (palTableAsset is not UDataTable palTable)
        {
            throw new InvalidOperationException($"Object {palAssetFilePath} was expected to be a {nameof(UDataTable)} but found {palTableAsset}");
        }

        PalParser palParser = new();

        return palTable.RowMap.Select(kv => palParser.TryParse(kv.Key.Text, kv.Value, out Pal? pal) ? pal : null).Where(p => p != null).Select(p => p!);
    }
}
