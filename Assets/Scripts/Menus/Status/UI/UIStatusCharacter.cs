﻿using System;
using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Menus.Status.UI.Stats;
using CryptoQuest.System;
using CryptoQuest.UI.Character;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Status.UI
{
    public class UIStatusCharacter : MonoBehaviour
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

        public HeroBehaviour InspectingHero
        {
            get
            {
                return _inspectingHero ??= Party.Slots[CurrentIndex].HeroBehaviour;
            }
        }
        private AttributeSystemBehaviour _inspectingAttributeSystem;
        private int _currentIndex;
        private IPartyController _party;
        private IPartyController Party => _party ??= ServiceProvider.GetService<IPartyController>();

        public int CurrentIndex
        {
            get => _currentIndex;
            private set
            {
                if (value < 0)
                    _currentIndex = Party.Size - 1;
                else if (value >= Party.Size)
                    _currentIndex = 0;
                else
                    _currentIndex = value;
            }
        }

        private void OnDisable()
        {
            CurrentIndex = 0;
        }

        public void ChangeCharacter(float direction)
        {
            var directionX = (int)direction;
            if (directionX == 0) return;
            InspectCharacter(GetTheNextValidMemberInParty(directionX));
        }

        /// <summary>
        /// Will wrap if needed
        /// </summary>
        /// <param name="direction">-1 for left and 1 for right</param>
        private HeroBehaviour GetTheNextValidMemberInParty(int direction)
        {
            PartySlot partySlot;
            do
            {
                CurrentIndex += direction;
                partySlot = Party.Slots[CurrentIndex];
            } while (!partySlot.IsValid());

            return partySlot.HeroBehaviour;
        }

        public void InspectCharacter(int index) => InspectCharacter(Party.Slots[index].HeroBehaviour);

        private void InspectCharacter(HeroBehaviour hero)
        {
            if (hero.IsValid() == false) return;
            _inspectingHero = hero;
            _inspectingAttributeSystem = _inspectingHero.GetComponent<AttributeSystemBehaviour>();
            RenderElementsStats(_inspectingAttributeSystem);
            SetupUI(_inspectingHero);
            _statusMenu.CharacterEquipmentsPanel.Show();
            InspectingCharacter?.Invoke(_inspectingHero);
        }

        private void SetupUI(HeroBehaviour hero)
        {
            var spec = hero.Spec;
            hero.TryGetComponent(out LevelSystem levelSystem);
            SetElement(hero.Element.Icon);
            SetLevel(levelSystem.Level);
            SetClass(hero.Class.Name);
            SetLocalizedName(hero.DetailsInfo.LocalizedName);

            SetAvatar(hero.Avatar);
            SetupExpUI(hero);
        }

        private void SetupExpUI(HeroBehaviour hero)
        {
            if (!hero.TryGetComponent<LevelSystem>(out var levelSystem)) return;
            SetMaxExp(levelSystem.GetNextLevelRequireExp());
            SetExp(levelSystem.GetCurrentLevelExp());
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