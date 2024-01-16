using CryptoQuest.Events;
using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Popups.Sagas
{
    public class ServerErrorPopup : ActionBase { }

    public class ServerErrorPopupSaga : SagaBase<ServerErrorPopup>
    {
        [SerializeField] private LocalizedString _errorMessage;
        [SerializeField] private LocalizedStringEventChannelSO _localizedErrorPopupEventSO;
        
        protected override void HandleAction(ServerErrorPopup ctx)
        {
            _localizedErrorPopupEventSO.RaiseEvent(_errorMessage);
        }
    }
}