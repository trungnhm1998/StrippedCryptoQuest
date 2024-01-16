using CryptoQuest.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.SocialPlatforms;

namespace CryptoQuest.UI.Popups
{
    public class AutoHidePopup : MonoBehaviour
    {
        [SerializeField] private float _autoHideDelay = 3f;
        [SerializeField] private UnityEvent _hidePopupEvent;
        [SerializeField] private LocalizedStringEventChannelSO _showPopupEventChannel;

        private void OnEnable()
        {
            _showPopupEventChannel.EventRaised += OnShowPopup;
        }

        private void OnDisable()
        {
            _showPopupEventChannel.EventRaised -= OnShowPopup;
            CancelInvoke(nameof(OnShowPopup));
        }

        private void OnShowPopup(LocalizedString _)
        {
            Invoke(nameof(HidePopup), _autoHideDelay);
        }

        private void HidePopup()
        {
            _hidePopupEvent.Invoke();
        }
    }
}