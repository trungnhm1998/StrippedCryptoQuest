using CryptoQuest.Events;
using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Popups.Sagas
{
    public class NotConnectWalletErrorPopup : ActionBase { }

    public class NotConnectWalletErrorPopupSaga : SagaBase<NotConnectWalletErrorPopup>
    {
        [SerializeField] private LocalizedString _errorMessage;
        [SerializeField] private LocalizedStringEventChannelSO _localizedErrorPopupEventSO;
        
        protected override void HandleAction(NotConnectWalletErrorPopup ctx)
        {
            _localizedErrorPopupEventSO.RaiseEvent(_errorMessage);
        }
    }
}