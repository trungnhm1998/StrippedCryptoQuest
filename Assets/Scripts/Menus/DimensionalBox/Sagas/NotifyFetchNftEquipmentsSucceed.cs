using System.Collections.Generic;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Events;

namespace CryptoQuest.Menus.DimensionalBox.Sagas
{
    public class NotifyFetchNftEquipmentsSucceed : SagaBase<FetchNftEquipmentSucceed>
    {
        protected override void HandleAction(FetchNftEquipmentSucceed ctx)
        {
            var ingameEquipments = new List<EquipmentResponse>();
            var inboxEquipments = new List<EquipmentResponse>();
            foreach (var equipmentResponse in ctx.Equipments)
            {
                if (equipmentResponse.nft != 1) continue;
                if (equipmentResponse.inGameStatus == (int)EEquipmentStatus.InGame)
                    ingameEquipments.Add(equipmentResponse);
                else
                    inboxEquipments.Add(equipmentResponse);
            }

            ActionDispatcher.Dispatch(new FetchIngameEquipmentsSuccess(ingameEquipments.ToArray()));
            ActionDispatcher.Dispatch(new FetchInboxEquipmentsSuccess(inboxEquipments.ToArray()));
        }
    }
}