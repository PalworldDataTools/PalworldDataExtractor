namespace PalworldDataExtractor.Models.Pals;

public class Pal
{
    public required string? TribeName { get; init; }
    public required string Name { get; init; }
    public required string DisplayName { get; init; }
    public required int Rarity { get; init; }
    public required string Size { get; init; }
    public required string ElementType1 { get; init; }
    public required string ElementType2 { get; init; }
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
