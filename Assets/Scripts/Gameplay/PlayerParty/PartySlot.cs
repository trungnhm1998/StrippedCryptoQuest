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

        private void Awake()
        {
            var child = transform.GetChild(0);
            Assert.IsNotNull(child, "Child is null");
            _character = child.GetComponent<ICharacter>();
            Assert.IsNotNull(_character, "Character is null");
        }

        /// <summary>
        /// Use this when change character's slot/order
        /// </summary>
        /// <param name="character"></param>
        public void SetCharacter(ICharacter character)
        {
            _character = character;
            _character.SetSlot(this);
        }

        public void Init(CharacterSpec character)
        {
            _character.Init(character);
        }
    }
}