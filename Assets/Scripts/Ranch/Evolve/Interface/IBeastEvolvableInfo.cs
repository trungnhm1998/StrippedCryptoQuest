using CryptoQuest.Beast;

namespace CryptoQuest.Ranch.Evolve.Interface
{
    public interface IBeastEvolvableInfo
    {
        public int BeforeStars { get; }
        public int AfterStars { get; }
        public float Rate { get; }
        public int Gold { get; }
        public float Metad { get; }
    }
}