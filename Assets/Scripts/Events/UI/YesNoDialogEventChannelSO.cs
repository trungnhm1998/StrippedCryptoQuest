using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Quest;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Events.UI
{
    public class YesNoDialogEventChannelSO : ScriptableObject
    {
        public event UnityAction<Action, Action> ShowEvent;
        public event UnityAction HideEvent;

        public void Show(Action yesButtonPressed, Action noButtonPressed)
        {
            OnShow(yesButtonPressed, noButtonPressed);
        }

        public void Hide()
        {
            OnHide();
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
    }
}
