using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.Events.UI.Dialogs
{
    public class OneButtonDialogEventChannelSO : ScriptableObject
    {
        public event UnityAction<Action> ShowEvent;
        public event UnityAction HideEvent;
        public event UnityAction<LocalizedString> SetMessageEvent;
        public event UnityAction<LocalizedString> SetButtonTextEvent;

        public void Show(Action buttonPressed)
        {
            OnShow(buttonPressed);
        }

        public void Hide()
        {
            OnHide();
        }

        public void SetMessage(LocalizedString message)
        {
            SetMessageEvent?.Invoke(message);
        }

        public void SetButtonText(LocalizedString message)
        {
            SetButtonTextEvent?.Invoke(message);
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

        private void OnShow(Action buttonPressed)
        {
            if (ShowEvent == null)
            {
                Debug.LogWarning("A request for showing dialog has been made, but no one listening.");
                return;
            }

            ShowEvent.Invoke(buttonPressed);
        }
    }
}
