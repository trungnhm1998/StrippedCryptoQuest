using CryptoQuest.Gameplay.Character;
using CryptoQuest.UI.Character;
using CryptoQuest.UI.Menu.Panels.Home;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle.PlayerParty
{
    public class UICharacterBattleInfo : MonoBehaviour, ICharacterInfo, ICharacterStats
    {
        [SerializeField] private LocalizeStringEvent _characterName;
        [SerializeField] private Image _avatar;
        [SerializeField] private UIAttributeBar _hpBar;
        [SerializeField] private UIAttributeBar _mpBar;
        [SerializeField] private AttributeChangeEvent _attributeChangeEvent;

        public CharacterSpec Member { get; private set; }

        public void Init(CharacterSpec member)
        {
            Member = member;
            Member.SetupUI(this);

            if (Member.CharacterComponent == null) return;
            _attributeChangeEvent.AttributeSystemReference = Member.CharacterComponent.AttributeSystem;
        }

        public void SetBattleAvatar(Sprite avatar)
        {
            _avatar.sprite = avatar;
        }

        public void SetLocalizedName(LocalizedString localizedName)
        {
            _characterName.StringReference = localizedName;
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

        public void SetAvatar(Sprite avatar) { }
        public void SetName(string charName) { }
        public void SetClass(LocalizedString localizedClassName) { }
        public void SetElement(Sprite elementIcon) { }
        public void SetLevel(int lvl) { }
        public void SetExp(float exp) { }
        public void SetMaxExp(int maxExp) { }
    }
}
