using CryptoQuest.Events;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Popups
{
    public class ErrorPopupController : BasePopupController<UIPopup>
    {
        [SerializeField] private StringEventChannelSO _errorPopupEventSO;
        [SerializeField] private LocalizedString _header;
        [SerializeField] private Color _headerColor;
        [SerializeField] private GameObject _background;

        private void OnEnable()
        {
            _errorPopupEventSO.EventRaised += ShowPopup;

            _inputManager.ClosePopupEvent += HideLastPopup;
        }
        
        private void OnDisable()
        {
            _errorPopupEventSO.EventRaised -= ShowPopup;

            _inputManager.ClosePopupEvent -= HideLastPopup;
        }

        private void ShowPopup(string body)
        {
            ShowPopup((UIPopup popup) => {
                popup.WithHeader(_header).SetHeaderColor(_headerColor)
                    .WithBody(body);
                    
                _background.SetActive(true);
            });
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