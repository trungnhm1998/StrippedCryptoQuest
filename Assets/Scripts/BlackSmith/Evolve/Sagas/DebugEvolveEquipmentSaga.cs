using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve.Sagas
{
    public class DebugEvolveEquipmentSaga : SagaBase<RequestEvolveEquipment>
    {
        [SerializeField] private float _delay = 1f;
        [SerializeField] private bool _shouldSuccess = true;

        private RequestEvolveEquipment _context;

        protected override void HandleAction(RequestEvolveEquipment ctx)
        {
            _context = ctx;

            ActionDispatcher.Dispatch(new ShowLoading());
            Invoke(nameof(SimulateDispatch), _delay);
        }

        private void SimulateDispatch()
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));

            if (_shouldSuccess)
            {
                IEvolvableEquipment successEquipment = new EvolvableEquipmentData()
                {
                    Equipment = _context.Equipment,
                    Level = _context.Equipment.Level,
                    Stars = _context.Equipment.Data.Stars + 1,
                };
                ActionDispatcher.Dispatch(new EvolveEquipmentSuccessAction(successEquipment));
                return;
            }

            IEvolvableEquipment failedEquipment = new EvolvableEquipmentData()
            {
                Equipment = _context.Equipment,
                Level = _context.Equipment.Level,
                Stars = _context.Equipment.Data.Stars,
            };
            ActionDispatcher.Dispatch(new EvolveEquipmentFailedAction(failedEquipment));
        }
    }
}
