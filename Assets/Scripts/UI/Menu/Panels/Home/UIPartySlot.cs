using CryptoQuest.Gameplay.Character;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Home
{
    public class UIPartySlot : MonoBehaviour
    {
        [SerializeField] private UICharacterInfo _characterInSlot;
        private bool _hasCharacter;
        public bool HasCharacter => _hasCharacter;
        public void Active(bool isValid)
        {
            _hasCharacter = isValid;
            _characterInSlot.gameObject.SetActive(_hasCharacter);
        }

        public void Init(CharacterSpec member, UICharacterInfo child)
        {
            _characterInSlot = child;
            _characterInSlot.Init(member);
        }
    }
}