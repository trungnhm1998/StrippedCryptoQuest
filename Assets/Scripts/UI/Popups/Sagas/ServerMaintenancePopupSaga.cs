using CryptoQuest.Events;
using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Popups.Sagas
{
    public class ServerMaintenancePopup : ActionBase { }

    public class ServerMaintenancePopupSaga : SagaBase<ServerMaintenancePopup>
    {
        [SerializeField] private LocalizedString _maintenanceMessage;
        [SerializeField] private LocalizedStringEventChannelSO _localizedMaintenancePopupEventSO;

        protected override void HandleAction(ServerMaintenancePopup ctx)
        {
            _localizedMaintenancePopupEventSO.RaiseEvent(_maintenanceMessage);
        }
    }
}