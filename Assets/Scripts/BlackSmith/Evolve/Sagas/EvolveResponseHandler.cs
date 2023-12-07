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
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace CryptoQuest.BlackSmith.Evolve.Sagas
{
    public class EvolveResponseHandler : SagaBase<EvolveResponsed>
    {
        private IInventoryController _inventoryController;
        private IPartyController _partyController;

        protected override void HandleAction(EvolveResponsed ctx)
        {
            var response = ctx.Response;

            int evolveStatus = response.data.success;

            ActionDispatcher.Dispatch(new RemoveEquipments(GetRemoveEquipments(ctx, evolveStatus)));

            ActionDispatcher.Dispatch(new AddEquipment(ctx.Response.data.newEquipment));

            switch (evolveStatus)
            {
                case 0:
                    ActionDispatcher.Dispatch(new EvolveEquipmentFailedAction());
                    break;
                case 1:
                    ActionDispatcher.Dispatch(new EvolveEquipmentSuccessAction());
                    break;
                default:
                    Debug.LogError("[EvolveEquipment]:: unknown success status: " + evolveStatus);
                    break;
            }
        }

        private List<IEquipment> GetRemoveEquipments(EvolveResponsed ctx, int evolveStatus)
        {
            // TODO: validate when server response has equipment id
            var updateEquipments = new List<IEquipment>
            {
                ctx.RequestContext.Material
            };
            
            if (evolveStatus == 1)
            {
                updateEquipments.Add(ctx.RequestContext.Equipment);
            }
            
            return updateEquipments;
        }
    }
}
