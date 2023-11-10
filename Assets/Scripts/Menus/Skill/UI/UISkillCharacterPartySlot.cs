using System;
using CryptoQuest.Battle.Components;
using CryptoQuest.Menu;
using CryptoQuest.UI.Menu.Character;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CryptoQuest.Menus.Skill.UI
{
    public class UISkillCharacterPartySlot : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private MultiInputButton _button;
        [SerializeField] private UnityEvent<HeroBehaviour> _onCharacterSelected;
        [SerializeField] private UICharacterInfoPanel _characterInSlot;
        [SerializeField] private GameObject _selectBorder;
        [SerializeField] private GameObject _selectBackground;

        public event Action<UISkillCharacterPartySlot> Selecting;

        public bool Interactable
        {
            set => _button.interactable = value;
        }

        public bool IsSelected
        {
            set => _selectBorder.SetActive(value);
        }

        public bool IsCharacterSelected { get; set; } = false;

        public HeroBehaviour Hero { get; private set; }

        public void Init(HeroBehaviour hero, int idxInParty)
        {
            Hero = hero;
            _characterInSlot.Init(hero);

            if (Hero.IsValid()) return;
            gameObject.SetActive(false);
        }

        public void Select()
        {
            if (Hero == null || !Hero.IsValid()) return;
            _button.Select();
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (Hero == null || !Hero.IsValid()) return;

            if (!IsCharacterSelected)
                _selectBackground.SetActive(true);
            else
                _selectBorder.SetActive(true);


            _onCharacterSelected?.Invoke(Hero);
            Selecting?.Invoke(this);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (Hero == null || !Hero.IsValid()) return;

            if (!IsCharacterSelected)
                _selectBackground.SetActive(false);
            else
                _selectBorder.SetActive(false);
        }

        public void EnableSelectBackground(bool isEnabled = true) => _selectBackground.SetActive(isEnabled);
    }
}