using System.Diagnostics.CodeAnalysis;
using CUE4Parse.FileProvider;
using CUE4Parse.UE4.Assets.Exports.Engine;
using CUE4Parse.UE4.Assets.Objects;
using PalworldDataExtractor.Abstractions.Pals;

namespace PalworldDataExtractor.Extractors;

class PalsExtractor
{
    readonly UDataTableReader _tableReader;

    public PalsExtractor(DefaultFileProvider provider)
    {
        _tableReader = new UDataTableReader(provider);
    }

    public async Task<IEnumerable<Pal>> ExtractPalsAsync()
    {
        UDataTable palTable = await _tableReader.Extract(@"Pal\Content\Pal\DataTable\Character\DT_PalMonsterParameter");
        return palTable.RowMap.Select(kv => TryParse(kv.Key.Text, kv.Value, out Pal? pal) ? pal : null).Where(p => p != null).Select(p => p!);
    }

    static bool TryParse(string property, FStructFallback obj, [NotNullWhen(true)] out Pal? pal)
    {
        FStructReader reader = new(obj);

        if (!reader.ParseBool("IsPal"))
        {
            pal = null;
            return false;
        }

        pal = new Pal
        {
            TribeName = ParseTribeName(reader, "Tribe"),
            Name = property,
            DisplayName = reader.ParseString("BPClass") ?? property,
            Rarity = reader.ParseInt("Rarity"),
            Size = ParseSize(reader, "Size") ?? "",
            ElementType1 = ParseElementType(reader, "ElementType1") ?? "",
            ElementType2 = ParseElementType(reader, "ElementType2") ?? "",
            Price = reader.ParseFloat("Price"),
            IsNocturnal = reader.ParseBool("Nocturnal"),
            IsEdible = reader.ParseBool("Edible"),
            IsBoss = reader.ParseBool("IsBoss"),
            IsTowerBoss = reader.ParseBool("IsTowerBoss"),
            IsPredator = reader.ParseBool("IsPredator"),
            CraftSpeed = reader.ParseInt("CraftSpeed"),
            CaptureRate = reader.ParseFloat("CaptureRateCorrect"),
            ExpRatio = reader.ParseFloat("ExpRatio"),
            SlowWalkSpeed = reader.ParseInt("SlowWalkSpeed"),
            WalkSpeed = reader.ParseInt("WalkSpeed"),
            RunSpeed = reader.ParseInt("RunSpeed"),
            RideSprintSpeed = reader.ParseInt("RideSprintSpeed"),
            TransportSpeed = reader.ParseInt("TransportSpeed"),
            MaxFullStomach = reader.ParseInt("MaxFullStomach"),
            FullStomachDecreaseRate = reader.ParseFloat("FullStomachDecreaseRate"),
            FoodAmount = reader.ParseInt("FoodAmount"),
            Stamina = reader.ParseInt("Stamina"),
            ViewingDistance = reader.ParseInt("ViewingDistance"),
            ViewingAngle = reader.ParseInt("ViewingAngle"),
            HearingRate = reader.ParseFloat("HearingRate"),
            Hp = reader.ParseInt("Hp"),
            MeleeAttack = reader.ParseInt("MeleeAttack"),
            ShotAttack = reader.ParseInt("ShotAttack"),
            Defense = reader.ParseInt("Defense"),
            Support = reader.ParseInt("Support"),
            EnemyReceiveDamageRate = reader.ParseFloat("EnemyReceiveDamageRate"),
            MaleProbability = reader.ParseInt("MaleProbability"),
            CombiRank = reader.ParseInt("CombiRank"),
            EmitFlame = reader.ParseInt("WorkSuitability_EmitFlame"),
            Watering = reader.ParseInt("WorkSuitability_Watering"),
            Seeding = reader.ParseInt("WorkSuitability_Seeding"),
            GenerateElectricity = reader.ParseInt("WorkSuitability_GenerateElectricity"),
            Handcraft = reader.ParseInt("WorkSuitability_Handcraft"),
            Collection = reader.ParseInt("WorkSuitability_Collection"),
            Deforest = reader.ParseInt("WorkSuitability_Deforest"),
            Mining = reader.ParseInt("WorkSuitability_Mining"),
            OilExtraction = reader.ParseInt("WorkSuitability_OilExtraction"),
            ProduceMedicine = reader.ParseInt("WorkSuitability_ProduceMedicine"),
            Cool = reader.ParseInt("WorkSuitability_Cool"),
            Transport = reader.ParseInt("WorkSuitability_Transport"),
            MonsterFarm = reader.ParseInt("WorkSuitability_MonsterFarm")
        };

        return true;
    }

    static string? ParseTribeName(FStructReader reader, string property) => reader.ParseEnumValue(property, "EPalTribeID::");
    static string? ParseSize(FStructReader reader, string property) => reader.ParseEnumValue(property, "EPalSizeType::");
    static string? ParseElementType(FStructReader reader, string property) => reader.ParseEnumValue(property, "EPalElementType::");
}
