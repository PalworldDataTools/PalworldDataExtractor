namespace PalworldDataExtractor.Abstractions.Breeding;

public class PalBreedingCombination
{
    public PalBreedingCombination(string parentTribeA, string parentTribeB, string childCharacterId)
    {
        ParentTribeA = parentTribeA;
        ParentTribeB = parentTribeB;
        ChildCharacterId = childCharacterId;
    }

    public string ParentTribeA { get; set; }
    public string ParentTribeB { get; set; }
    public string ChildCharacterId { get; set; }
}
