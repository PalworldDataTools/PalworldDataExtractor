using CUE4Parse_Conversion.Textures;
using CUE4Parse.FileProvider;
using CUE4Parse.UE4.Assets.Exports.Engine;
using CUE4Parse.UE4.Assets.Exports.Texture;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Objects.UObject;
using SkiaSharp;

namespace PalworldDataExtractor.Extractors;

public class PalIconsExtractor
{
    readonly DefaultFileProvider _provider;

    public PalIconsExtractor(DefaultFileProvider provider)
    {
        _provider = provider;
    }

    public async Task<IReadOnlyDictionary<string, byte[]>> ExtractPalsAsync()
    {
        const string palAssetFilePath = @"Game\Pal\DataTable\Character\DT_PalCharacterIconDataTable.DT_PalCharacterIconDataTable";

        UDataTable palTable = await _provider.LoadObjectAsync<UDataTable>(palAssetFilePath);

        Dictionary<string, byte[]> dictionary = new();
        foreach (KeyValuePair<FName, FStructFallback> row in palTable.RowMap)
        {
            dictionary[row.Key.Text] = GetIcon(row.Value);
        }

        return dictionary;
    }

    static byte[] GetIcon(FStructFallback props)
    {
        UTexture2D? texture = props.GetOrDefault<UTexture2D?>("Icon");
        return texture?.Decode()?.Encode(SKEncodedImageFormat.Png, 80).ToArray() ?? Array.Empty<byte>();
    }
}
