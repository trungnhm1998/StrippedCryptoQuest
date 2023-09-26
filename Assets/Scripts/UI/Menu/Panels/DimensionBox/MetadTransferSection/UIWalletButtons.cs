using System;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox
{
    public enum EWalletType
    {
        IngameWallet = 0,
        WebWallet = 1,
    }
    [RequireComponent(typeof(Button))]
    public class UIWalletButtons : MonoBehaviour
    {
        public event UnityAction<UIWalletButtons> SelectedEvent;
        [SerializeField] private GameObject _selectedBackground;
        [SerializeField] private GameObject _arrowSelected;
        [field: SerializeField] public Button Button { get; private set; }
        [field: SerializeField] public EWalletType WalletType { get; private set; }

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
    }
}