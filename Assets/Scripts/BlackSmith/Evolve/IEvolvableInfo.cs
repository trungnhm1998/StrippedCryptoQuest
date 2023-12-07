namespace CryptoQuest.BlackSmith.Evolve
{
    public interface IEvolvableInfo
    {
        int Rarity { get; }
        int BeforeStars { get; }
        int AfterStars { get; }
        int Gold { get; }
        float Metad { get; }
        int Rate { get; }
    }
}
