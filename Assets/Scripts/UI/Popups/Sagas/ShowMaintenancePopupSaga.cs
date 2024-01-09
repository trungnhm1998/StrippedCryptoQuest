using System;
using CryptoQuest.Events;
using CryptoQuest.UI.Popups.Objects;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Popups.Sagas
{
    public class ShowMaintenancePopupSaga : SagaBase<ServerMaintaining>
    {
        [SerializeField] private LocalizedString _maintenanceMessage;
        [SerializeField] private LocalizedStringEventChannelSO _localizedMaintenancePopupEventSO;

        private const string DESIRED_FORMAT = "yyyy/MM/dd HH:mm";

        protected override void HandleAction(ServerMaintaining ctx)
        {
            MaintainenceResponse response =
                JsonConvert.DeserializeObject<MaintainenceResponse>(ctx.RequestException.Response);

            DateTime startAt = DateTime.Parse(response.data.startAt);
            DateTime endAt = DateTime.Parse(response.data.endAt);

            string formattedStartAt = startAt.ToString(DESIRED_FORMAT);
            string formattedEndAt = endAt.ToString(DESIRED_FORMAT);

            LocalizedString value = new LocalizedString
            {
                TableReference = _maintenanceMessage.TableReference,
                TableEntryReference = _maintenanceMessage.TableEntryReference,
                Arguments = new object[] { formattedStartAt, formattedEndAt }
            };

            _localizedMaintenancePopupEventSO.RaiseEvent(value);
        }
    }
}