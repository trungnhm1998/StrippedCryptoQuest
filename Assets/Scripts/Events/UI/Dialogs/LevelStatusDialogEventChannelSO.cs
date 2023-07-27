using CryptoQuest.Gameplay.Quest;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Events.UI.Dialogs
{
    public class LevelStatusDialogEventChannelSO : ScriptableObject
    {
        public event UnityAction<LevelStatusDialogData> ShowEvent;
        public event UnityAction HideEvent;

        public void Show(LevelStatusDialogData levelStatusDialogData)
        {
            OnShow(levelStatusDialogData);
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

        private void OnShow(LevelStatusDialogData levelStatusDialogData)
        {
            if (levelStatusDialogData == null)
            {
                Debug.LogWarning("A request for showing dialog has been made, but not dialogue were provided.");
                return;
            }

            if (ShowEvent == null)
            {
                Debug.LogWarning("A request for showing dialog has been made, but no one listening.");
                return;
            }

            ShowEvent.Invoke(levelStatusDialogData);
        }
    }
}
