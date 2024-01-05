using CryptoQuest.Events;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace CryptoQuest.UI.Popups
{
    public class MaintenancePopupController : BasePopupController<UIPopup>
    {
        [SerializeField] private StringEventChannelSO _maintenancePopupEventSO;
        [SerializeField] private LocalizedStringEventChannelSO _localizedMaintenancePopupEventSO;
        [SerializeField] private LocalizedString _header;
        [SerializeField] private Color _headerColor;
        [SerializeField] private GameObject _background;

        private void OnEnable()
        {
            _maintenancePopupEventSO.EventRaised += ShowPopup;
            _localizedMaintenancePopupEventSO.EventRaised += ShowPopup;

            _inputManager.ClosePopupEvent += HideLastPopup;
        }

        private void OnDisable()
        {
            _maintenancePopupEventSO.EventRaised -= ShowPopup;
            _localizedMaintenancePopupEventSO.EventRaised -= ShowPopup;

            _inputManager.ClosePopupEvent -= HideLastPopup;
        }

        private void ShowPopup(string body)
        {
            ShowPopup(popup =>
                SetupMaintenancePopup(popup)
                    .WithBody(body));
        }

        private void ShowPopup(LocalizedString body)
        {
            ShowPopup(popup =>
                SetupMaintenancePopup(popup)
                    .WithBody(body));
        }

        private UIPopup SetupMaintenancePopup(UIPopup popup)
        {
            popup.WithHeader(_header)
                .SetHeaderColor(_headerColor);

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