using CUE4Parse.UE4.Assets.Objects;
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
        string? valueString = ParseString(property);
        if (valueString == null || !valueString.StartsWith(prefix))
        {
            return null;
        }

        return valueString[prefix.Length..];
    }

    public string? ParseString(string property) => _obj.TryGetValue(out FName value, property) ? value.Text : null;
    public int ParseInt(string property) => _obj.TryGetValue(out int value, property) ? value : 0;
    public float ParseFloat(string property) => _obj.TryGetValue(out float value, property) ? value : 0f;
    public bool ParseBool(string property) => _obj.TryGetValue(out bool value, property) && value;
}
