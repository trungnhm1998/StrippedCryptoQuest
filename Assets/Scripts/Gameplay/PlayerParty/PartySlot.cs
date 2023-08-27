using CryptoQuest.Gameplay.Character;
using UnityEngine;
using UnityEngine.Assertions;

namespace CryptoQuest.Gameplay.PlayerParty
{
    /// <summary>
    /// I do not want to re-initialize the character when the party is sorted, Simply change the parent of the character
    /// to another slot.
    /// </summary>
    public class PartySlot : MonoBehaviour
    {
        private ICharacter _character;
        public ICharacter Character => _character;

        private void Awake()
        {
            var child = transform.GetChild(0);
            Assert.IsNotNull(child, "Child is null");
            _character = child.GetComponent<ICharacter>();
            Assert.IsNotNull(_character, "Character component not found");
        }

        public void Init(CharacterSpec character)
        {
            _character.Init(character);
        }
        
        public bool IsValid()
        {
            return _character != null && _character.Spec.IsValid();
        }
    }
}