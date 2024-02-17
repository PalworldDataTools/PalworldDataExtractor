namespace PalworldDataExtractor.Models.Pals;

public class PalCombat
{
    public required int Hp { get; init; }
    public required int MeleeAttack { get; init; }
    public required int ShotAttack { get; init; }
    public required int Defense { get; init; }
    public required int Support { get; init; }
    public required float EnemyReceiveDamageRate { get; init; }
}
