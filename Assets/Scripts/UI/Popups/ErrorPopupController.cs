using CryptoQuest.Events;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Popups
{

    public class ErrorPopupController : BasePopupController<UIPopup>
    {
        [SerializeField] private StringEventChannelSO _errorPopupEventSO;
        [SerializeField] private LocalizedStringEventChannelSO _localizedErrorPopupEventSO;
        [SerializeField] private LocalizedString _header;
        [SerializeField] private Color _headerColor;
        [SerializeField] private GameObject _background;

        private void OnEnable()
        {
            _errorPopupEventSO.EventRaised += ShowPopup;
            _localizedErrorPopupEventSO.EventRaised += ShowPopup;

            _inputManager.ClosePopupEvent += HideLastPopup;
        }
        
        private void OnDisable()
        {
            _errorPopupEventSO.EventRaised -= ShowPopup;
            _localizedErrorPopupEventSO.EventRaised -= ShowPopup;
            
            _inputManager.ClosePopupEvent -= HideLastPopup;
        }

        private void ShowPopup(string body)
        {
            ShowPopup((UIPopup popup) => {
                SetupErrorPopup(popup).WithBody(body);
            });
        }

        private void ShowPopup(LocalizedString body)
        {
            ShowPopup((UIPopup popup) => {
                SetupErrorPopup(popup).WithBody(body);
            });
        }

        private UIPopup SetupErrorPopup(UIPopup popup)
        {
            popup.WithHeader(_header).SetHeaderColor(_headerColor);
                
            _background.SetActive(true);
            return popup;
        }

        private void HideLastPopup()
        {
            if (_popups.Count <= 0) return;
            var popup = _popups[^1];
            Hide(popup);
            if (IsPopupsEmpty) _background.SetActive(false);
        }
    }
}