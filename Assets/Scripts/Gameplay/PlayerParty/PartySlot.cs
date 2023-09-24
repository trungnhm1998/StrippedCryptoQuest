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
        [SerializeField] private CharacterBehaviourBase _characterComponent;

        public CharacterBehaviourBase CharacterComponent => _characterComponent;
        private CharacterSpec _characterSpec;

        public void Init(CharacterSpec character)
        {
            if (character.IsValid() == false) return;
            _characterSpec = character;
            _characterComponent.Init(character);
        }

        public bool IsValid()
        {
            return _characterComponent != null && _characterSpec.IsValid();
        }
    }
}