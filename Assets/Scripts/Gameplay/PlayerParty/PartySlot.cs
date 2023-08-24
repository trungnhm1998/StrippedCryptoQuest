using CryptoQuest.Gameplay.Character;
using UnityEngine;
using UnityEngine.Assertions;

namespace CryptoQuest.Gameplay.PlayerParty
{
    public interface IPartySlot
    {
        public ICharacter Character { get; }
    }

    public class PartySlot : MonoBehaviour, IPartySlot
    {
        private ICharacter _character;

        public ICharacter Character { get; }

        private void Awake()
        {
            _character = GetComponentInChildren<ICharacter>();
            // make sure the prefab is correct that has the ICharacter component
            Assert.IsNotNull(_character, "Character is null");
        }

        /// <summary>
        /// When the party first loaded, the <see cref="ICharacter"/> in this slot will be initialized
        /// </summary>
        /// <param name="characterMetaData"></param>
        /// 
        /// <summary>
        /// Use this when change character's slot/order
        /// </summary>
        /// <param name="character"></param>
        public void SetCharacter(ICharacter character)
        {
            _character = character;
        }

        public void Init(CharacterSpec character)
        {
            _character.Init(character);
        }
    }
}