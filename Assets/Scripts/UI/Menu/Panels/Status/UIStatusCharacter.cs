using System;
using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using CryptoQuest.UI.Menu.Panels.Home;
using CryptoQuest.UI.Menu.Panels.Status.Stats;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using CryptoQuest.UI.Character;

namespace CryptoQuest.UI.Menu.Panels.Status
{
    public class UIStatusCharacter : MonoBehaviour, ICharacterInfo
    {
        public event Action<HeroBehaviour> InspectingCharacter;
        [SerializeField] private UIStatusMenu _statusMenu;

        [Header("Character Info UI References")]
        [SerializeField] private Image _avatar;

        [SerializeField] private Image _characterElement;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private LocalizeStringEvent _localizedName;
        [SerializeField] private LocalizeStringEvent _localizedClassName;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private List<UIElementAttribute> _elementAttributes;
        [SerializeField] private UIAttributeBar _expBar;

        private string _lvlTxtFormat = string.Empty; // could made this into static
        private HeroBehaviour _inspectingHero;
        private AttributeSystemBehaviour _inspectingAttributeSystem;
        private int _currentIndex;
        private IPartyController _party;

        private int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                _currentIndex = value switch
                {
                    < 0 => PartyConstants.MAX_PARTY_SIZE - 1,
                    >= PartyConstants.MAX_PARTY_SIZE => 0,
                    _ => value
                };
            }
        }

        private void Start()
        {
            _party = ServiceProvider.GetService<IPartyController>();
        }

        private void OnEnable()
        {
            _statusMenu.Show += InspectFirstCharacter;
        }

        private void OnDisable()
        {
            _statusMenu.Show -= InspectFirstCharacter;
        }

        private void InspectFirstCharacter()
        {
            InspectCharacter(_party.Slots[0].HeroBehaviour);
        }

        public void ChangeCharacter(float direction)
        {
            var directionX = (int)direction;
            if (directionX == 0) return;
            CurrentIndex += directionX;
            InspectCharacter(GetTheNextValidMemberInParty(directionX));
        }

        /// <summary>
        /// Will wrap if needed
        /// </summary>
        /// <param name="direction">-1 for left and 1 for right</param>
        private HeroBehaviour GetTheNextValidMemberInParty(int direction)
        {
            var memberInParty = _party.Slots[CurrentIndex];
            while (memberInParty.IsValid() == false)
            {
                CurrentIndex += direction;
                memberInParty = _party.Slots[CurrentIndex];
            }

            return memberInParty.HeroBehaviour;
        }

        private void InspectCharacter(HeroBehaviour hero)
        {
            if (hero.IsValid() == false) return;
            _inspectingHero = hero;
            _inspectingAttributeSystem = _inspectingHero.GetComponent<AttributeSystemBehaviour>();
            RenderElementsStats(_inspectingAttributeSystem);
            _inspectingHero.SetupUI(this);
            InspectingCharacter?.Invoke(_inspectingHero);
        }

        private void RenderElementsStats(AttributeSystemBehaviour attributeSystem)
        {
            for (int i = 0; i < _elementAttributes.Count; i++)
            {
                var elementUI = _elementAttributes[i];
                elementUI.SetStats(attributeSystem);
            }
        }

        #region Setup UI

        public void SetLocalizedName(LocalizedString localizedName)
        {
            _localizedName.StringReference = localizedName;
        }

        public void SetName(string charName)
        {
            _name.text = charName;
        }

        public void SetClass(LocalizedString localizedClassName)
        {
            _localizedClassName.StringReference = localizedClassName;
        }

        public void SetAvatar(Sprite avatar)
        {
            _avatar.sprite = avatar;
        }

        public void SetElement(Sprite elementIcon)
        {
            _characterElement.sprite = elementIcon;
        }

        public void SetExp(float exp)
        {
            _expBar.SetValue(exp);
        }

        public void SetMaxExp(int maxExp)
        {
            _expBar.SetMaxValue(maxExp);
        }

        public void SetLevel(int lvl)
        {
            if (string.IsNullOrEmpty(_lvlTxtFormat))
                _lvlTxtFormat = _level.text;
            _level.text = string.Format(_lvlTxtFormat, lvl);
        }

        #endregion
    }
}