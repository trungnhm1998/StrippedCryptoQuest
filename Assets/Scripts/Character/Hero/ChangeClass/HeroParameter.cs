using CryptoQuest.Gameplay;
using UnityEngine;

namespace CryptoQuest.Character.Hero.ChangeClass
{
    public class HeroParameter : ScriptableObject
    {
        [field: SerializeField] public CharacterClass Class { get; private set; }
        [field: SerializeField] public StatsDef Stats { get; private set; }
    }
}
