using System;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu
{
    public class UITabButton : MonoBehaviour
    {
        public event Action<UITabButton> Selected;
        public event Action<UITabButton> Deselected;
        [SerializeField] private GameObject _tabPanel;
        public GameObject ManagedPanel => _tabPanel;

        private Button _button;
        private Button Button => _button ??= GetComponent<Button>();

        public bool Interactable
        {
            get => Button.interactable;
            set => Button.interactable = value;
        }

        private void OnEnable() => Button.onClick.AddListener(Select);

        private void OnDisable() => Button.onClick.RemoveListener(Select);

        public void Select()
        {
            Button.Select();
            Selected?.Invoke(this);
        }

        public void Deselect() => Deselected?.Invoke(this);
    }
}