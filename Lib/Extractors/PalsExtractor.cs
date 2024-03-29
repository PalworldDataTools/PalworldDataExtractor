﻿using System.Diagnostics.CodeAnalysis;
using CUE4Parse.FileProvider;
using CUE4Parse.UE4.Assets.Exports.Engine;
using CUE4Parse.UE4.Assets.Objects;
using PalworldDataExtractor.Abstractions.Pals;
using PalworldDataExtractor.Readers;

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
        UDataTable palTable = await _tableReader.ExtractAsync(@"Pal\Content\Pal\DataTable\Character\DT_PalMonsterParameter");
        return palTable.RowMap.Select((kv, index) => TryParse(index, kv.Key.Text, kv.Value, out Pal? pal) ? pal : null).Where(p => p != null).Select(p => p!);
    }

    static bool TryParse(int index, string property, FStructFallback obj, [NotNullWhen(true)] out Pal? pal)
    {
        FStructReader reader = new(obj);

        if (!reader.ParseBool("IsPal"))
        {
            pal = null;
            return false;
        }

        pal = new Pal
        {
            GameIndex = index,
            TribeName = reader.ParseTribeName("Tribe"),
            Name = property,
            ZukanIndex = reader.ParseInt("ZukanIndex"),
            ZukanIndexSuffix = reader.ParseString("ZukanIndexSuffix") ?? "",
            Rarity = reader.ParseInt("Rarity"),
            Size = reader.ParseSize("Size") ?? "",
            ElementType1 = reader.ParseElementType("ElementType1") ?? "",
            ElementType2 = reader.ParseElementType("ElementType2") ?? "",
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
}
