using System.Collections.Generic;
using CryptoQuest.Core;
using CryptoQuest.Networking;
using CryptoQuest.Networking.Actions;
using TinyMessenger;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Title
{
    public class UISignInPanel : MonoBehaviour
    {
        private const int SIGN_IN_BUTTON_INDEX = 2;
        [SerializeField] private Credentials _credentials;
        [field: SerializeField] public List<Selectable> Selectables { get; private set; }
        [SerializeField] private TMP_InputField _emailInputField;
        [SerializeField] private TMP_InputField _passwordInputField;
        private int _currenSelectIndex = 0;

        private bool _hasResult = true;
        private TinyMessageSubscriptionToken _authFailed;

        private void OnEnable()
        {
            Invoke(nameof(SelectFirstSelectable), 0);
            if (string.IsNullOrEmpty(_credentials.Email) ||
                string.IsNullOrEmpty(_credentials.Password))
            {
                return;
            }

            _currenSelectIndex = SIGN_IN_BUTTON_INDEX;
            _emailInputField.text = _credentials.Email;
            _passwordInputField.text = _credentials.Password;

            _authFailed = ActionDispatcher.Bind<AuthenticateFailed>(UpdateResult);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_authFailed);
        }

        private void SelectFirstSelectable() => Selectables[_currenSelectIndex].Select();

        public void HandleDirection(float value)
        {
            _currenSelectIndex -= (int)value;
            _currenSelectIndex = _currenSelectIndex < 0 ? Selectables.Count - 1 : _currenSelectIndex;
            _currenSelectIndex = _currenSelectIndex >= Selectables.Count ? 0 : _currenSelectIndex;
            var selectable = Selectables[_currenSelectIndex];
            selectable.Select();
        }

        private void UpdateResult(ActionBase _)
        {
            _hasResult = true;
        }

        public void OnSignInButtonPressed()
        {
            if (_hasResult == false) return;
            _hasResult = false;
            ActionDispatcher.Dispatch(new AuthenticateUsingEmail(_emailInputField.text, _passwordInputField.text));
        }

        public void RegisterPressed()
        {
            if (_hasResult == false) return;
            _hasResult = false;
            ActionDispatcher.Dispatch(new RegisterEmailAction(_emailInputField.text, _passwordInputField.text));
        }
    }
}