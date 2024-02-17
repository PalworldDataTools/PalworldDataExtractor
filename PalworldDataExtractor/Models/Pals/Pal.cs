namespace PalworldDataExtractor.Models.Pals;

public class PalTribe
{
    public required string Name { get; init; }
    public required IReadOnlyCollection<Pal> Pals { get; init; }
}

public class Pal
{
    public required string? TribeName { get; init; }
    public required string Name { get; init; }
    public required string DisplayName { get; init; }
    public required int Rarity { get; init; }
    public required PalSizeType Size { get; init; }
    public required PalElementType ElementType1 { get; init; }
    public required PalElementType ElementType2 { get; init; }
    public required PalGenusCategoryType GenusCategory { get; init; }
    public required PalOrganizationType OrganizationType { get; init; }
    public required PalWeaponType WeaponType { get; init; }
    public required float Price { get; init; }

    public required bool IsNocturnal { get; init; }
    public required bool IsEdible { get; init; }
    public required bool IsBoss { get; init; }
    public required bool IsTowerBoss { get; init; }
    public required bool IsPredator { get; init; }

    public required int CraftSpeed { get; init; }
    public required float CaptureRate { get; init; }
    public required float ExpRatio { get; init; }
    public required int SlowWalkSpeed { get; init; }
    public required int WalkSpeed { get; init; }
    public required int RunSpeed { get; init; }
    public required int RideSprintSpeed { get; init; }
    public required int TransportSpeed { get; init; }
    public required int MaxFullStomach { get; init; }
    public required float FullStomachDecreaseRate { get; init; }
    public required int FoodAmount { get; init; }
    public required int Stamina { get; init; }
    public required int ViewingDistance { get; init; }
    public required int ViewingAngle { get; init; }
    public required float HearingRate { get; init; }

    public required PalCombat Combat { get; init; }
    public required PalBreeding Breeding { get; init; }
    public required PalWork Work { get; init; }
}

public class PalCombat
{
    public required int Hp { get; init; }
    public required int MeleeAttack { get; init; }
    public required int ShotAttack { get; init; }
    public required int Defense { get; init; }
    public required int Support { get; init; }
    public required float EnemyReceiveDamageRate { get; init; }
}

public class PalBreeding
{
    public required int MaleProbability { get; init; }
    public required int CombiRank { get; init; }
}

public class PalWork
{
    public required int EmitFlame { get; init; }
    public required int Watering { get; init; }
    public required int Seeding { get; init; }
    public required int GenerateElectricity { get; init; }
    public required int Handcraft { get; init; }
    public required int Collection { get; init; }
    public required int Deforest { get; init; }
    public required int Mining { get; init; }
    public required int OilExtraction { get; init; }
    public required int ProduceMedicine { get; init; }
    public required int Cool { get; init; }
    public required int Transport { get; init; }
    public required int MonsterFarm { get; init; }
}

public enum PalSizeType
{
    None = 0,
    S,
    M,
    L,
    XL
}

public enum PalElementType
{
    None = 0,
    Dark
}

public enum PalGenusCategoryType
{
    None = 0,
    FourLegged
}

public enum PalOrganizationType
{
    None = 0
}

public enum PalWeaponType
{
    None = 0
}
