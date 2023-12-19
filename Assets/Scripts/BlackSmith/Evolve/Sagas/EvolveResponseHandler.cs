using CryptoQuest.Item.Equipment;
using CryptoQuest.Sagas;
using CryptoQuest.Sagas.Profile;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using System.Collections.Generic;
using CryptoQuest.Sagas.Equipment;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve.Sagas
{
    public class EvolveResponseHandler : SagaBase<EvolveResponsed>
    {   
        private IEquipmentResponseConverter _responseConverter;

        protected override void HandleAction(EvolveResponsed ctx)
        {
            _responseConverter ??= ServiceProvider.GetService<IEquipmentResponseConverter>();
            var response = ctx.Response;

            int evolveStatus = response.data.success;

            ActionDispatcher.Dispatch(new RemoveEquipments(GetRemoveEquipments(ctx, evolveStatus)));
            ActionDispatcher.Dispatch(new UpdateWalletAction(response));

            switch (evolveStatus)
            {
                case 0:
                    ActionDispatcher.Dispatch(new EvolveEquipmentFailedAction(ctx.RequestContext.Equipment));
                    break;
                case 1:
                    ActionDispatcher.Dispatch(new ResolveResponseSuccessAction(ctx.Response.data.newEquipment));
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
