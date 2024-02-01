using System;
using CryptoQuest.Events;
using CryptoQuest.UI.Actions;
using CryptoQuest.UI.Popups.Objects;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Popups.Sagas
{
    public class UnauthorizedPopup : ActionBase { }

    public class UnauthorizedPopupSaga : SagaBase<UnauthorizedPopup>
    {
        [SerializeField] private LocalizedString _message;
        [SerializeField] private LocalizedStringEventChannelSO _localizedErrorPopupEventSO;
        
        protected override void HandleAction(UnauthorizedPopup ctx)
        {
            _localizedErrorPopupEventSO.RaiseEvent(_message);
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }
    }
}