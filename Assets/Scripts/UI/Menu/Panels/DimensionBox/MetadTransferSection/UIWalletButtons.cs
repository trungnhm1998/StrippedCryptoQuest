using System;
using CryptoQuest.Menu;
using CryptoQuest.Networking.Menu.DimensionBox;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.MetadTransferSection
{
    [RequireComponent(typeof(Button))]
    public abstract class UIWalletButtons : MonoBehaviour
    {
        public event UnityAction<UIWalletButtons> SelectedEvent;
        [SerializeField] private GameObject _selectedBackground;
        [SerializeField] private GameObject _arrowSelected;
        [SerializeField] protected MetadAPI _metadAPI;
        [field: SerializeField] public LocalizedString SendingMessage { get; private set; }

        [field: SerializeField] public Button Button { get; private set; }

        private void OnValidate()
        {
            Button = GetComponent<Button>();
        }

        public void OnClicked()
        {
            _selectedBackground.SetActive(true);
            _arrowSelected.SetActive(true);
            Button.interactable = false;
            SelectedEvent?.Invoke(this);
        }

        public void SetHighlight(bool isActive)
        {
            _selectedBackground.SetActive(isActive);
            _arrowSelected.SetActive(isActive);
        }

        public abstract void Send(float value);
    }
}