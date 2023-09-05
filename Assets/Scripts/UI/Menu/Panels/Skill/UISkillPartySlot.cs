using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Character;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    public class UISkillPartySlot : MonoBehaviour
    {
        [SerializeField] private UISkillCharInfo _characterInSlot;
        private bool _hasCharacter;
        public bool HasCharacter => _hasCharacter;

        public void Active(bool isValid)
        {
            _hasCharacter = isValid;
            _characterInSlot.gameObject.SetActive(_hasCharacter);
        }

        public void Init(CharacterSpec member)
        {
            _characterInSlot.Init(member);
        }
    }
}
