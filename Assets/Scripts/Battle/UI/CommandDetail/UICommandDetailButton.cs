using System;
using CryptoQuest.Battle.Events;
using CryptoQuest.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Pool;

namespace CryptoQuest.Battle.UI.CommandDetail
{
    public class UICommandDetailButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;
        [SerializeField] private LocalizeStringEvent _labelStringEvent;
        [SerializeField] protected TMP_Text _value;
        [SerializeField] private MultiInputButton _button;

        public IObjectPool<UICommandDetailButton> Pool { get; set; }
        private ButtonInfoBase _buttonInfo;
        private int _index;

        private SelectedDetailButtonEvent _selectedEvent;
        private DeSelectedDetailButtonEvent _deSelectedEvent;

        public virtual void Init(ButtonInfoBase info, int index)
        {
            _index = index;
            CreateEvents();

            if (!string.IsNullOrEmpty(info.Label))
            {
                _label.text = info.Label;
            }

            if (info.LocalizedLabel != null)
            {
                _labelStringEvent.StringReference = info.LocalizedLabel;
            }
            _value.text = info.Value;
            _buttonInfo = info;
            SetupButtonInfo();
        }

        public void Select()
        {
            if (!_button.interactable) return;
            _button.Select();
        }

        protected virtual void OnEnable()
        {
            _button.Selected += OnSelectButton;
            _button.DeSelected += OnDeSelectButton;
        }

        protected virtual void OnDisable()
        {
            _button.Selected -= OnSelectButton;
            _button.DeSelected -= OnDeSelectButton;
        }

        private void CreateEvents()
        {
            _selectedEvent = new SelectedDetailButtonEvent() { Index = _index };
            _deSelectedEvent = new DeSelectedDetailButtonEvent() { Index = _index };
        }

        private void SetupButtonInfo()
        {
            _button.interactable = _buttonInfo.IsInteractable; 
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(_buttonInfo.OnHandleClick);
        }

        private void OnSelectButton()
        {
            BattleEventBus.RaiseEvent<SelectedDetailButtonEvent>(_selectedEvent);
        }

        private void OnDeSelectButton()
        {
            BattleEventBus.RaiseEvent<DeSelectedDetailButtonEvent>(_deSelectedEvent);
        }

        public void ReleaseToPool()
        {
            Pool.Release(this);
        }
    }
}