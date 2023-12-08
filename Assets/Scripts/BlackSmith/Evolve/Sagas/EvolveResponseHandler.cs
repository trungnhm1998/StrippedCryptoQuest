using CryptoQuest.Item.Equipment;
using IndiGames.Core.Events;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve.Sagas
{
    public class EvolveResponseHandler : SagaBase<EvolveResponsed>
    {
        protected override void HandleAction(EvolveResponsed ctx)
        {
            var response = ctx.Response;

            int evolveStatus = response.data.success;

            ActionDispatcher.Dispatch(new RemoveEquipments(GetRemoveEquipments(ctx, evolveStatus)));

            ActionDispatcher.Dispatch(new AddEquipment(ctx.Response.data.newEquipment));

            switch (evolveStatus)
            {
                case 0:
                    // TODO: need a way to parse data from server, so we can get the new equipment stats
                    var failedEquipment = new EvolvableEquipmentData()
                    {
                        Equipment = ctx.RequestContext.Equipment,
                        Level = ctx.RequestContext.Equipment.Level,
                        Stars = ctx.RequestContext.Equipment.Data.Stars,
                    };

                    ActionDispatcher.Dispatch(new EvolveEquipmentFailedAction(failedEquipment));
                    break;
                case 1:
                    var successEquipment = new EvolvableEquipmentData()
                    {
                        Equipment = ctx.RequestContext.Equipment,
                        Level = ctx.Response.data.newEquipment.lv,
                        Stars = ctx.Response.data.newEquipment.star,
                    };

                    ActionDispatcher.Dispatch(new EvolveEquipmentSuccessAction(successEquipment));
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
