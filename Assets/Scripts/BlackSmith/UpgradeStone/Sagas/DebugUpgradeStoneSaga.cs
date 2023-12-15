using CryptoQuest.Item.MagicStone;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.BlackSmith.UpgradeStone.Sagas
{
    public class DebugUpgradeStoneSag : SagaBase<RequestUpgradeStone>
    {
        [SerializeField] private float _delay = 1f;
        [SerializeField] private bool _isStimulateSuccess = true;
        private RequestUpgradeStone _context;

        protected override void HandleAction(RequestUpgradeStone ctx)
        {
            _context = ctx;
            ActionDispatcher.Dispatch(new ShowLoading());
            Invoke(nameof(DebugResponseDispatch), _delay);
        }

        private void DebugResponseDispatch()
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));

            if (_isStimulateSuccess)
            {
                MagicStoneData data = new MagicStoneData()
                {
                    ID = 123,
                    Level = _context.Stones[0].Level + 1,
                    Def = _context.Stones[0].Definition,
                    Passives = _context.Stones[0].Passives,
                };
                MagicStone stone = new MagicStone()
                {
                    Data = data,
                };

                ActionDispatcher.Dispatch(new ResponseUpgradeStoneSuccess(stone));
                return;
            }

            ActionDispatcher.Dispatch(new ResponseUpgradeStoneFailed());
        }
    }
}