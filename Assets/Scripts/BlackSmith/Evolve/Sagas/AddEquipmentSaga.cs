using CryptoQuest.BlackSmith.Evolve.UI;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using System;
using UniRx;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve.Sagas
{
    public class AddEquipmentSaga : SagaBase<AddEquipment>
    {
        protected override void HandleAction(AddEquipment ctx)
        {
            // TODO: convert new equipment ctx.EquipmentData to item in inventory
        }
    }
}
