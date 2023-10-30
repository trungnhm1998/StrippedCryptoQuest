using System;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Title
{
    public class UINameConfirmPanel : MonoBehaviour
    {
        public event Action YesPressed;
        public event Action NoPressed;
        [SerializeField] private Selectable _yesButton;

        private void OnEnable() => Invoke(nameof(SelectYesButton), 0f);
        private void SelectYesButton() => _yesButton.Select();
        public void ConfirmPlayerName() => YesPressed?.Invoke();
        public void NoButtonPressed_BackToNameInput() => NoPressed?.Invoke();
    }
}