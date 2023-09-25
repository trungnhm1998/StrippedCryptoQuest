using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Character;
using UnityEngine;

namespace CryptoQuest.Gameplay.PlayerParty
{
    /// <summary>
    /// I do not want to re-initialize the character when the party is sorted, Simply change the parent of the character
    /// to another slot.
    /// </summary>
    public class PartySlot : MonoBehaviour
    {
        private CharacterSpec _characterSpec;
        private IHero _hero;
        public IHero CharacterComponent => _hero;

        private void Awake()
        {
            _hero = GetComponentInChildren<IHero>();
        }

        public void Init(CharacterSpec character)
        {
            if (character.IsValid() == false) return;
            _characterSpec = character;
            _hero.Init(character);
        }

        public bool IsValid()
        {
            return _hero != null && _characterSpec.IsValid();
        }
    }
}