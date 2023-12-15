using CryptoQuest.Item.MagicStone;
using CryptoQuest.Sagas.MagicStone;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.BlackSmith.UpgradeStone.Sagas
{
    public class DebugGetStoneSaga : SagaBase<FetchProfileMagicStonesAction>
    {
        [SerializeField] private MagicStoneInventory _debugInventory;
        [SerializeField] private float _delay = 1f;
        [SerializeField] private bool _isStimulateSuccess = true;

        protected override void HandleAction(FetchProfileMagicStonesAction ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            Invoke(nameof(DebugResponseDispatch), _delay);
        }

        private void DebugResponseDispatch()
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));

            if (_isStimulateSuccess)
            {
                ActionDispatcher.Dispatch(new ResponseGetMagicStonesSucceeded(_debugInventory.MagicStones));
                return;
            }

            ActionDispatcher.Dispatch(new ResponseGetMagicStonesFailed());
        }
    }
}