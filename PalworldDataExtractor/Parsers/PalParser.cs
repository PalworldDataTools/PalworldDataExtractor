using System.Diagnostics.CodeAnalysis;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Objects.UObject;
using PalworldDataExtractor.Models.Pals;

namespace PalworldDataExtractor.Parsers;

public class PalParser
{
    public bool TryParse(string property, FStructFallback obj, [NotNullWhen(true)] out Pal? pal)
    {
        if (!obj.TryGetValue(out bool value, "IsPal") || !value)
        {
            pal = null;
            return false;
        }

        pal = new Pal
        {
            TribeName = ParseTribeName(obj, "Tribe"),
            Name = property,
            DisplayName = ParseString(obj, "BPClass") ?? property,
            Rarity = ParseInt(obj, "Rarity"),
            Size = ParseSize(obj, "Size"),
            ElementType1 = ParseElementType(obj, "ElementType1"),
            ElementType2 = ParseElementType(obj, "ElementType2"),
            GenusCategory = ParseGenusCategory(obj, "GenusCategory"),
            OrganizationType = ParseOrganization(obj, "OrganizationType"),
            WeaponType = ParseWeapon(obj, "Weapon"),
            Price = ParseInt(obj, "Price"),
            IsNocturnal = ParseBool(obj, "Nocturnal"),
            IsEdible = ParseBool(obj, "Edible"),
            IsBoss = ParseBool(obj, "IsBoss"),
            IsTowerBoss = ParseBool(obj, "IsTowerBoss"),
            IsPredator = ParseBool(obj, "IsPredator"),
            CraftSpeed = ParseInt(obj, "CraftSpeed"),
            CaptureRate = ParseInt(obj, "CaptureRate"),
            ExpRatio = ParseInt(obj, "ExpRatio"),
            SlowWalkSpeed = ParseInt(obj, "SlowWalkSpeed"),
            WalSpeed = ParseInt(obj, "WalSpeed"),
            RunSpeed = ParseInt(obj, "RunSpeed"),
            RideSprintSpeed = ParseInt(obj, "RideSprintSpeed"),
            TransportSpeed = ParseInt(obj, "TransportSpeed"),
            MaxFullStomach = ParseInt(obj, "MaxFullStomach"),
            FullStomachDecreaseRate = ParseInt(obj, "FullStomachDecreaseRate"),
            FoodAmount = ParseInt(obj, "FoodAmount"),
            Stamina = ParseInt(obj, "Stamina"),
            ViewingDistance = ParseInt(obj, "ViewingDistance"),
            ViewingAngle = ParseInt(obj, "ViewingAngle"),
            HearingRate = ParseInt(obj, "HearingRate"),
            Combat = new PalCombat
            {
                Hp = ParseInt(obj, "Hp"),
                MeleeAttack = ParseInt(obj, "MeleeAttack"),
                ShotAttack = ParseInt(obj, "ShotAttack"),
                Defense = ParseInt(obj, "Defense"),
                Support = ParseInt(obj, "Support"),
                EnemyReceiveDamageRate = ParseInt(obj, "EnemyReceiveDamageRate")
            },
            Breeding = new PalBreeding
            {
                MaleProbability = ParseInt(obj, "MaleProbability"),
                CombiRank = ParseInt(obj, "CombiRank")
            },
            Work = new PalWork
            {
                EmitFlame = ParseInt(obj, "WorkSuitability_EmitFlame"),
                Watering = ParseInt(obj, "WorkSuitability_Watering"),
                Seeding = ParseInt(obj, "WorkSuitability_Seeding"),
                GenerateElectricity = ParseInt(obj, "WorkSuitability_GenerateElectricity"),
                Handcraft = ParseInt(obj, "WorkSuitability_Handcraft"),
                Collection = ParseInt(obj, "WorkSuitability_Collection"),
                Deforest = ParseInt(obj, "WorkSuitability_Deforest"),
                Mining = ParseInt(obj, "WorkSuitability_Mining"),
                OilExtraction = ParseInt(obj, "WorkSuitability_OilExtraction"),
                ProduceMedicine = ParseInt(obj, "WorkSuitability_ProduceMedicine"),
                Cool = ParseInt(obj, "WorkSuitability_Cool"),
                Transport = ParseInt(obj, "WorkSuitability_Transport"),
                MonsterFarm = ParseInt(obj, "WorkSuitability_MonsterFarm")
            }
        };

        return true;
    }

    static string? ParseTribeName(FStructFallback obj, string property)
    {
        string? tribeName = ParseString(obj, property);
        if (tribeName == null)
        {
            return null;
        }

        if (tribeName.StartsWith("EPalTribeID::"))
        {
            return tribeName[13..];
        }

        return tribeName;
    }

    static PalSizeType ParseSize(FStructFallback obj, string property) => ParseEnumValue(obj, property, "EPalSizeType::", PalSizeType.None);

    static PalElementType ParseElementType(FStructFallback obj, string property) => ParseEnumValue(obj, property, "EPalElementType::", PalElementType.None);

    static PalGenusCategoryType ParseGenusCategory(FStructFallback obj, string property) => ParseEnumValue(obj, property, "EPalGenusCategoryType", PalGenusCategoryType.None);

    static PalOrganizationType ParseOrganization(FStructFallback obj, string property) => ParseEnumValue(obj, property, "EPalOrganizationType", PalOrganizationType.None);

    static PalWeaponType ParseWeapon(FStructFallback obj, string property) => ParseEnumValue(obj, property, "EPalWeaponType", PalWeaponType.None);

    static TEnum ParseEnumValue<TEnum>(FStructFallback obj, string property, string prefix, TEnum defaultValue)
    {
        string? valueString = ParseString(obj, property);
        if (valueString == null || !valueString.StartsWith(prefix))
        {
            return defaultValue;
        }

        string typeName = valueString[prefix.Length..];
        if (Enum.TryParse(typeof(TEnum), typeName, true, out object? type))
        {
            return (TEnum)type;
        }

        return defaultValue;
    }

    static string? ParseString(FStructFallback obj, string property) => obj.TryGetValue(out FName value, property) ? value.Text : null;
    static int ParseInt(FStructFallback obj, string property) => obj.TryGetValue(out int value, property) ? value : 0;
    static bool ParseBool(FStructFallback obj, string property) => obj.TryGetValue(out bool value, property) && value;
}
