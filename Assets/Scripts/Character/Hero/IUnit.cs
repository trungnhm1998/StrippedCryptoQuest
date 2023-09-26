using CryptoQuest.Character.Attributes;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Character;

namespace CryptoQuest.Character.Hero
{
    public interface IUnit
    {
        public enum ECategory
        {
            Normal,
            Special,
        }

        /// <summary>
        /// The NFT ID of the character
        /// </summary>
        public string ID { get; }
        public ECategory Category { get; }
        public Origin Origin { get; }
        public CharacterClass Class { get; }
        public Elemental Element { get; }
        public StatsDef Stats { get; }
        public bool IsNft { get; }
    }
}