namespace CryptoQuest.BlackSmith.Evolve
{
    public class EvolvableInfo : IEvolvableInfo
    {
        public int Rarity { get; set; }
        public int BeforeStars { get; set; }
        public int AfterStars { get; set; }
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public int Gold { get; set; }
        public float Metad { get; set; }
        public int Rate { get; set; }
    }
}