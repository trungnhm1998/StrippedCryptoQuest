using System;
using CryptoQuest.Battle.Components;
using CryptoQuest.Menu;
using CryptoQuest.UI.Menu.Character;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CryptoQuest.Menus.Item.UI
{
    public class UIItemCharacterPartySlot : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private MultiInputButton _button;
        [SerializeField] private UnityEvent<HeroBehaviour> _onCharacterSelected;
        [SerializeField] private UICharacterInfoPanel _characterInSlot;
        [SerializeField] private GameObject _selectBorder;
        [SerializeField] private GameObject _selectBackground;

        public event Action<UIItemCharacterPartySlot> Selecting;
        public event Action<UIItemCharacterPartySlot> Selected;

        public bool Interactable
        {
            set => _button.interactable = value;
        }

        public bool IsSelected
        {
            set => _selectBackground.SetActive(value);
        }

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

        public void OnSelected() => Selected?.Invoke(this);

        public void OnSelect(BaseEventData eventData)
        {
            if (Hero == null || !Hero.IsValid()) return;
            _selectBorder.SetActive(true);
            _onCharacterSelected?.Invoke(Hero);
            Selecting?.Invoke(this);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (Hero == null || !Hero.IsValid()) return;
            _selectBorder.SetActive(false);
        }
    }
}