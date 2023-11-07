using System;
using CryptoQuest.Core;
using CryptoQuest.Networking.Actions;
using CryptoQuest.UI.Common;
using TinyMessenger;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.UI.Title
{
    public class UISocialPanel : MonoBehaviour
    {
        [SerializeField] private Button[] _buttons;
        private TinyMessageSubscriptionToken _authFailed;

        private void OnEnable()
        {
            _authFailed = ActionDispatcher.Bind<AuthenticateFailed>(_ => EnableAllButtonsAndSelectLastSelected());
            // prevent immediate button pressed because LoginFailed state also using submit event
            Invoke(nameof(DelaySelectButton), 0);
        }

        private void DelaySelectButton()
        {
            EnableAllButtonsAndSelectLastSelected();
            GetComponentInChildren<SelectButtonOnEnable>().Select();
        }

        private void OnDisable()
        {
            EnableAllButtonsAndSelectLastSelected(false);
            ActionDispatcher.Unbind(_authFailed);
        }

        public void RequestFacebookLogin() =>
            PreventDoubleDispatch(() => ActionDispatcher.Dispatch(new LoginUsingFacebook()));

        public void RequestTwitterLogin() =>
            PreventDoubleDispatch(() => ActionDispatcher.Dispatch(new LoginUsingTwitter()));

        public void RequestGmailLogin() =>
            PreventDoubleDispatch(() => ActionDispatcher.Dispatch(new LoginUsingGoogle()));

        public void RequestEmailAndPasswordLogin() =>
            PreventDoubleDispatch(() => ActionDispatcher.Dispatch(new LoginUsingEmail()));

        private void PreventDoubleDispatch(Action callback)
        {
            if (isActiveAndEnabled == false) return;
            EnableAllButtonsAndSelectLastSelected(false);
            callback();
        }

        private void EnableAllButtonsAndSelectLastSelected(bool isEnabled = true)
        {
            if (!isEnabled) CacheLastSelectedButton();
            foreach (var button in _buttons) button.interactable = isEnabled;
            if (_lastSelectedButton is null || !enabled) return;
            _lastSelectedButton.Select();
        }

        private Button _lastSelectedButton;

        private void CacheLastSelectedButton()
        {
            if (EventSystem.current is null) return;
            var selectedGameObject = EventSystem.current.currentSelectedGameObject;
            if (selectedGameObject == null) return;
            var lastSelectedButton = selectedGameObject.GetComponent<Button>();
            if (lastSelectedButton == null) return;
            var index = Array.IndexOf(_buttons, lastSelectedButton);
            if (index < 0) return;
            _lastSelectedButton = lastSelectedButton;
        }
    }
}