using System;
using CryptoQuest.UI.Menu;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CryptoQuest.UI
{
    public class UIInputConfirmPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _content;

        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;

        public UnityAction OnYesButtonPressed;
        public UnityAction OnNoButtonPressed;

        private void Awake()
        {
            _yesButton.Select();
        }

        public void YesButtonPressed()
        {
            OnYesButtonPressed?.Invoke();
        }

        public void NoButtonPressed()
        {
            OnNoButtonPressed?.Invoke();
        }

        public void Show() => _content.SetActive(true);

        public void Hide() => _content.SetActive(false);
    }
}