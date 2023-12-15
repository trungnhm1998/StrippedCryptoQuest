using System.Collections;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Sagas.Profile;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve.Sagas
{
    public class ResolveResponseSuccessSaga : SagaBase<ResolveResponseSuccessAction>
    {
        private IInventoryController _inventoryController;
        private IEquipmentResponseConverter _responseConverter;

        protected override void HandleAction(ResolveResponseSuccessAction ctx)
        {
            if (ctx.EquipmentData == null) return;

            _inventoryController ??= ServiceProvider.GetService<IInventoryController>();
            _responseConverter ??= ServiceProvider.GetService<IEquipmentResponseConverter>();

            StartCoroutine(CoConvertEquipment(ctx));
        }

        private IEnumerator CoConvertEquipment(ResolveResponseSuccessAction ctx)
        {
            var equipment = _responseConverter.Convert(ctx.EquipmentData);

            yield return new WaitUntil(() => equipment.IsValid());

            _inventoryController.Add(equipment);

            ActionDispatcher.Dispatch(new EvolveEquipmentSuccessAction(equipment));
        }
    }
}
