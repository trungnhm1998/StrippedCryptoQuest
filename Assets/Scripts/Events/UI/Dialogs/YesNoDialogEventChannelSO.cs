using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.Events.UI.Dialogs
{
    public class YesNoDialogEventChannelSO : ScriptableObject
    {
        public event UnityAction<Action, Action> ShowEvent;
        public event UnityAction HideEvent;
        public event UnityAction<LocalizedString> SetMessageEvent;

        public void Show(Action yesButtonPressed, Action noButtonPressed)
        {
            OnShow(yesButtonPressed, noButtonPressed);
        }

        public void Hide()
        {
            OnHide();
        }

        public void SetMessage(LocalizedString message)
        {
            OnSetMessage(message);
        }

        private void OnHide()
        {
            if (HideEvent == null)
            {
                Debug.LogWarning("A request for hiding dialog has been made, but no one listening.");
                return;
            }

            HideEvent.Invoke();
        }

        private void OnShow(Action yesButtonPressed, Action noButtonPressed)
        {
            if (ShowEvent == null)
            {
                Debug.LogWarning("A request for showing dialog has been made, but no one listening.");
                return;
            }

            ShowEvent.Invoke(yesButtonPressed, noButtonPressed);
        }

        private void OnSetMessage(LocalizedString message)
        {
            SetMessageEvent?.Invoke(message);
        }
    }
}
