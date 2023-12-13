using CryptoQuest.Ranch.Evolve.Interface;

namespace CryptoQuest.Ranch.Evolve
{
    public class BeastEvolvableInfo : IBeastEvolvableInfo
    {
        public int BeforeStars { get; set; }
        public int AfterStars { get; set; }
        public float Rate { get; set; }
        public int Gold { get; set; }
        public float Metad { get; set; }
    }
}