using CUE4Parse.FileProvider;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Exports.Engine;

namespace PalworldDataExtractor.Extractors;

public class UDataTableReader
{
    readonly DefaultFileProvider _provider;

    public UDataTableReader(DefaultFileProvider provider)
    {
        _provider = provider;
    }

    public async Task<UDataTable> Extract(string file)
    {
        List<UObject> objects = (await _provider.LoadAllObjectsAsync(file)).ToList();
        if (objects.Count == 0)
        {
            throw new InvalidOperationException($"Could not find pal table in asset {file}");
        }

        UObject dataTableObject = objects.First();
        if (dataTableObject is not UDataTable dataTable)
        {
            throw new InvalidOperationException($"Object {file} was expected to be a {nameof(UDataTable)} but found {dataTableObject}");
        }

        return dataTable;
    }
}
