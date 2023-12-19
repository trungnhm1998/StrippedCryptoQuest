using System.Collections;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Sagas.Equipment;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve.Sagas
{
    public class ResolveResponseSuccessSaga : SagaBase<ResolveResponseSuccessAction>
    {
        private IEquipmentResponseConverter _responseConverter;

        protected override void HandleAction(ResolveResponseSuccessAction ctx)
        {
            if (ctx.EquipmentData == null) return;

            _responseConverter ??= ServiceProvider.GetService<IEquipmentResponseConverter>();

            StartCoroutine(CoConvertEquipment(ctx));
        }

        private IEnumerator CoConvertEquipment(ResolveResponseSuccessAction ctx)
        {
            var equipment = _responseConverter.Convert(ctx.EquipmentData);

            yield return new WaitUntil(() => equipment.IsValid());

            ActionDispatcher.Dispatch(new AddEquipmentAction(equipment));
            ActionDispatcher.Dispatch(new EvolveEquipmentSuccessAction(equipment));
        }
    }
}
