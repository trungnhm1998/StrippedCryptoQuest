using CryptoQuest.Events;
using CryptoQuest.Sagas;
using IndiGames.Core.Events;
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
            if (_popups.Count >= 1) return;
            ShowPopup(popup =>
                SetupMaintenancePopup(popup)
                    .WithBody(body));
        }

        private void ShowPopup(LocalizedString body)
        {
            if (_popups.Count >= 1) return;
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
            Hide(_popups[0]);
            if (IsPopupsEmpty) _background.SetActive(false);

            ActionDispatcher.Dispatch(new GoToTitleAction());
        }
    }
}