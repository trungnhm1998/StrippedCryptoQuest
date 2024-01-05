using CryptoQuest.Events;
using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Popups.Sagas
{
    public class ClientMaintenancePopup : ActionBase { }

    public class ClientMaintenancePopupSaga : SagaBase<ClientMaintenancePopup>
    {
        [SerializeField] private LocalizedString _maintenanceMessage;
        [SerializeField] private LocalizedStringEventChannelSO _localizedMaintenancePopupEventSO;

        protected override void HandleAction(ClientMaintenancePopup ctx)
        {
            _localizedMaintenancePopupEventSO.RaiseEvent(_maintenanceMessage);
        }
    }
}