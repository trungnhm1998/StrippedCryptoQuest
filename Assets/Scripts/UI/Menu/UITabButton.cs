using System;
using CryptoQuest.Menu;
using UnityEngine;

namespace CryptoQuest.UI.Menu
{
    public class UITabButton : MonoBehaviour
    {
        public event Action<UITabButton> Pressed;
        public event Action<UITabButton> Selected;
        public event Action<UITabButton> Deselected;
        [SerializeField] private GameObject _tabPanel;
        public GameObject ManagedPanel => _tabPanel;

        private MultiInputButton _button;
        private MultiInputButton Button => _button ??= GetComponent<MultiInputButton>();

        public bool Interactable
        {
            get => Button.interactable;
            set => Button.interactable = value;
        }

        private void OnEnable()
        {
            Button.onClick.AddListener(OnPressed);
            Button.Selected += OnSelect;
        }

        private void OnDisable()
        {
            Button.onClick.RemoveListener(OnPressed);
            Button.Selected -= OnSelect;
        }

        private void OnPressed() => Pressed?.Invoke(this);

        private void OnSelect() => Selected?.Invoke(this);

        private void OnDeselect() => Deselected?.Invoke(this);

        public void Select()
        {
            Button.Select();
            OnSelect();
        }
    }
}