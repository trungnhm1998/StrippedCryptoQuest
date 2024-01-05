using CryptoQuest.Events;
using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Popups.Sagas
{
    public class ShowMaintenancePopupSaga : SagaBase<ServerMaintaining>
    {
        [SerializeField] private LocalizedString _maintenanceMessage;
        [SerializeField] private LocalizedStringEventChannelSO _localizedMaintenancePopupEventSO;

        protected override void HandleAction(ServerMaintaining ctx)
        {
            _localizedMaintenancePopupEventSO.RaiseEvent(_maintenanceMessage);
        }
    }
}