using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.Skill;
using CryptoQuest.UI.Character;
using CryptoQuest.UI.Menu.Panels.Home;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace CryptoQuest
{
    public class UISkillCharInfo : MonoBehaviour, ICharacterInfo, ICharacterStats
    {
        [SerializeField] private Image _avatar;
        [SerializeField] private UIAttributeBar _hpBar;
        [SerializeField] private UIAttributeBar _mpBar;
        [SerializeField] private AttributeChangeEvent _attributeChangeEvent;

        private CharacterSpec _memberInSlot;

        public void Init(CharacterSpec member)
        {
            _memberInSlot = member;
            member.SetupUI(this);

            _attributeChangeEvent.AttributeSystemReference = _memberInSlot.CharacterComponent.AttributeSystem;
            _memberInSlot.CharacterComponent.AttributeSystem.UpdateAttributeValues();
        }

        public void SetAvatar(Sprite avatar)
        {
            _avatar.sprite = avatar;
        }

        public void SetCurrentHp(float currentHp)
        {
            _hpBar.SetValue(currentHp);
        }

        public void SetMaxHp(float maxHp)
        {
            _hpBar.SetMaxValue(maxHp);
        }

        public void SetCurrentMp(float currentMp)
        {
            _mpBar.SetValue(currentMp);
        }

        public void SetMaxMp(float maxMp)
        {
            _mpBar.SetMaxValue(maxMp);
        }

        public void SetLocalizedName(LocalizedString localizedName) { }
        public void SetName(string charName) { }
        public void SetClass(LocalizedString localizedClassName) { }
        public void SetElement(Sprite elementIcon) { }
        public void SetLevel(int lvl) { }
        public void SetExp(float exp) { }
        public void SetMaxExp(int maxExp) { }
    }
}
