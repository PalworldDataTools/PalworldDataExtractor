using System.Diagnostics.CodeAnalysis;
using CUE4Parse.FileProvider;
using CUE4Parse.UE4.Assets.Exports.Engine;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Objects.UObject;
using PalworldDataExtractor.Abstractions.Breeding;
using PalworldDataExtractor.Readers;

namespace PalworldDataExtractor.Extractors;

public class UniquePalBreedingCombinationExtractor
{
    readonly UDataTableReader _tableReader;

    public UniquePalBreedingCombinationExtractor(DefaultFileProvider provider)
    {
        _tableReader = new UDataTableReader(provider);
    }

    public async Task<IEnumerable<PalBreedingCombination>> ExtractUniquePalBreedingCombinationsAsync()
    {
        UDataTable palTable = await _tableReader.ExtractAsync(@"Pal\Content\Pal\DataTable\Character\DT_PalCombiUnique");

        List<PalBreedingCombination> result = new();

        foreach (KeyValuePair<FName, FStructFallback> kv in palTable.RowMap)
        {
            if (!TryParse(kv.Value, out PalBreedingCombination? combi))
            {
                continue;
            }

            result.Add(combi);
        }

        return result;
    }

    static bool TryParse(FStructFallback obj, [NotNullWhen(true)] out PalBreedingCombination? combi)
    {
        FStructReader reader = new(obj);

        string? parentTribeA = reader.ParseTribeName("ParentTribeA");
        string? parentTribeB = reader.ParseTribeName("ParentTribeB");
        string? childCharacterId = reader.ParseString("ChildCharacterID");

        if (parentTribeA == null || parentTribeB == null || childCharacterId == null)
        {
            combi = null;
            return false;
        }

        combi = new PalBreedingCombination(parentTribeA, parentTribeB, childCharacterId);
        return true;
    }
}
