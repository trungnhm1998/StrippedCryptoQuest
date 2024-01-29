using CryptoQuest.Battle.Components;
using UnityEngine;

namespace CryptoQuest.Menus.Home.UI
{
    public class UIPartySlot : MonoBehaviour
    {
        [SerializeField] private UICharacterInfo _characterInSlot;
        private bool _hasCharacter;
        public bool HasCharacter => _hasCharacter;
        public void Active(bool isValid)
        {
            _hasCharacter = isValid;
            _characterInSlot.transform.parent.gameObject.SetActive(_hasCharacter);
        }

        public void Init(HeroBehaviour member, UICharacterInfo child)
        {
            _characterInSlot = child;
            _characterInSlot.Init(member);
        }
    }
}