using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Objects.Core.i18N;
using CUE4Parse.UE4.Objects.UObject;

namespace PalworldDataExtractor.Readers;

public class FStructReader
{
    readonly FStructFallback _obj;

    public FStructReader(FStructFallback obj)
    {
        _obj = obj;
    }

    public string? ParseEnumValue(string property, string prefix)
    {
        string? valueString = ParseString(property) ?? _obj.GetOrDefault<FName>(property).Text;
        if (valueString == "" || valueString == "None" || !valueString.StartsWith(prefix))
        {
            return null;
        }

        return valueString[prefix.Length..];
    }

    public string? ParseString(string property) => (string?)_obj.GetOrDefault<string>(property) ?? ((FText?)_obj.GetOrDefault<FText>(property))?.Text;
    public int ParseInt(string property) => _obj.GetOrDefault<int>(property);
    public float ParseFloat(string property) => _obj.GetOrDefault<float>(property);
    public bool ParseBool(string property) => _obj.GetOrDefault<bool>(property);
}
