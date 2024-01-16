using CryptoQuest.Events;
using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.Localization;


namespace CryptoQuest.UI.Popups.Sagas
{
    public class ClientErrorPopup : ActionBase { }

    public class ClientErrorPopupSaga : SagaBase<ClientErrorPopup>
    {
        [SerializeField] private LocalizedString _errorMessage;
        [SerializeField] private LocalizedStringEventChannelSO _localizedErrorPopupEventSO;
        
        protected override void HandleAction(ClientErrorPopup ctx)
        {
            _localizedErrorPopupEventSO.RaiseEvent(_errorMessage);
        }
    }
}