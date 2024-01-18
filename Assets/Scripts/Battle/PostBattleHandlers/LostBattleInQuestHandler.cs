using CryptoQuest.Battle.Events;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Battle.PostBattleHandlers
{
    public class LostBattleInQuestHandler : PostBattleManager
    {
        [SerializeField] private BattleResultEventSO _battleLostEvent;

        protected override ResultSO.EState ResultState => ResultSO.EState.LoseInQuest;

        protected override void HandleResult()
        {
            _battleLostEvent.RaiseEvent(_battleBus.CurrentBattlefield);
            ActionDispatcher.Dispatch(new RestorePartyAction());
        }
    }
}