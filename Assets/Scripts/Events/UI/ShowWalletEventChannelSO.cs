using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Events.UI
{
    [CreateAssetMenu(menuName = "Crypto Quest/Events/ShowWalletEventChannelSO", fileName = "ShowWalletEventChannelSO")]
    public class ShowWalletEventChannelSO : ScriptableObject
    {
        public event UnityAction<bool, bool, bool> ShowEvent;
        public event UnityAction HideEvent;

        public void Show(bool showGolds = true, bool showDiamonds = false, bool showSouls = false)
        {
            OnShow(showGolds, showDiamonds, showSouls);
        }

        public void Hide()
        {
            OnHide();
        }

        private void OnShow(bool showGolds, bool showDiamonds, bool showSouls)
        {
            if (ShowEvent == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");
                return;
            }

            ShowEvent.Invoke(showGolds, showDiamonds, showSouls);
        }

        private void OnHide()
        {
            if (HideEvent == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");
                return;
            }

            HideEvent.Invoke();
        }
    }
}