using System.Collections.Generic;
using CryptoQuest.Actions;
using CryptoQuest.Networking;
using IndiGames.Core.Events;
using TinyMessenger;
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

        private bool _hasResult = true;
        private TinyMessageSubscriptionToken _authFailed;

        private void OnEnable()
        {
            SelectFirstSelectable();
            if (!string.IsNullOrEmpty(_credentials.Email))
            {
                _emailInputField.text = _credentials.Email;
            }
            if (!string.IsNullOrEmpty(_credentials.Password))
            {
                _passwordInputField.text = _credentials.Password;
            }
        }

        private void Awake()
        {
            _authFailed = ActionDispatcher.Bind<AuthenticateFailed>(UpdateResult);
        }

        private void OnDestroy()
        {
            if (_authFailed != null) ActionDispatcher.Unbind(_authFailed);
        }

        private void SelectFirstSelectable()
        {
            _currenSelectIndex = 0;
            Selectables[_currenSelectIndex].Select();
        }

        public void HandleDirection(float value)
        {
            _currenSelectIndex -= (int)value;
            _currenSelectIndex = _currenSelectIndex < 0 ? Selectables.Count - 1 : _currenSelectIndex;
            _currenSelectIndex = _currenSelectIndex >= Selectables.Count ? 0 : _currenSelectIndex;
            Selectables[_currenSelectIndex].Select();
        }

        private void UpdateResult(ActionBase _)
        {
            _hasResult = true;
        }

        public void OnSignInButtonPressed()
        {
            if (_hasResult == false) return;
            _hasResult = false;
            SelectFirstSelectable();
            ActionDispatcher.Dispatch(new AuthenticateUsingEmail(_emailInputField.text, _passwordInputField.text));
        }
    }
}