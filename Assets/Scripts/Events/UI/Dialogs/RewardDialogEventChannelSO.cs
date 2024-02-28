using CryptoQuest.UI.Dialogs.RewardDialog;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Events.UI.Dialogs
{
    public class RewardDialogEventChannelSO : ScriptableObject
    {
        public event UnityAction<RewardDialogData> ShowEvent;
        public event UnityAction HideEvent;

        public void Show(RewardDialogData rewardDialogData)
        {
            OnShow(rewardDialogData);
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

        private void OnShow(RewardDialogData rewardDialogData)
        {
            if (rewardDialogData.Loots == null)
            {
                Debug.LogWarning("A request for showing dialog has been made, but not dialogue were provided.");
                return;
            }

            if (ShowEvent == null)
            {
                Debug.LogWarning("A request for showing dialog has been made, but no one listening.");
                return;
            }

            ShowEvent.Invoke(rewardDialogData);
        }
    }
}