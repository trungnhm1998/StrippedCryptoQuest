using System.Collections;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Sagas.Equipment;
using CryptoQuest.Sagas.Profile;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve.Sagas
{
    public class DebugEvolveEquipmentSaga : SagaBase<RequestEvolveEquipment>
    {
        [SerializeField] private float _delay = 1f;
        [SerializeField] private bool _shouldSuccess = true;
        [SerializeField] private string _fakeResponse;
        private RequestEvolveEquipment _context;
        
        private IEquipmentResponseConverter _responseConverter;

        protected override void HandleAction(RequestEvolveEquipment ctx)
        {
            _context = ctx;
            _responseConverter ??= ServiceProvider.GetService<IEquipmentResponseConverter>();

            ActionDispatcher.Dispatch(new ShowLoading());
            Invoke(nameof(SimulateDispatch), _delay);
        }

        private void SimulateDispatch()
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            var response = JsonConvert.DeserializeObject<EvolveResponse>(_fakeResponse);
            response.data.success = _shouldSuccess ? 1 : 0; 
            ActionDispatcher.Dispatch(new EvolveResponsed(response, _context));
        }
    }
}
