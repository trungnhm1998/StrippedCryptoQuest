using System.Collections.Generic;
using CryptoQuest.Core;
using CryptoQuest.Networking;
using CryptoQuest.Networking.Actions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Title
{
    public class UISignInPanel : MonoBehaviour
    {
        [SerializeField] private Credentials _credentials;
        [field: SerializeField] public List<Selectable> Selectables { get; private set; }
        [SerializeField] private TMP_InputField _emailInputField;
        [SerializeField] private TMP_InputField _passwordInputField;
        private int _currenSelectIndex = 0;

        private void OnEnable()
        {
            Invoke(nameof(SelectFirstSelectable), 0);

            if (string.IsNullOrEmpty(_credentials.Email) ||
                string.IsNullOrEmpty(_credentials.Password))
                return;

            _emailInputField.text = _credentials.Email;
            _passwordInputField.text = _credentials.Password;
        }

        private void SelectFirstSelectable() => Selectables[0].Select();

        public void HandleDirection(float value)
        {
            _currenSelectIndex -= (int)value;
            _currenSelectIndex = _currenSelectIndex < 0 ? Selectables.Count - 1 : _currenSelectIndex;
            _currenSelectIndex = _currenSelectIndex >= Selectables.Count ? 0 : _currenSelectIndex;
            var selectable = Selectables[_currenSelectIndex];
            selectable.Select();
        }

        public void OnSignInButtonPressed()
        {
            ActionDispatcher.Dispatch(new AuthenticateUsingEmail(_emailInputField.text, _passwordInputField.text));
        }
    }
}