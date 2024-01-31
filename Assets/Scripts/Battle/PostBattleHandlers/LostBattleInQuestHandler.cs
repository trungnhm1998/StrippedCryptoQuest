using IndiGames.Core.Events;

namespace CryptoQuest.Battle.PostBattleHandlers
{
    public class LostBattleInQuestHandler : PostBattleManager
    {
        protected override ResultSO.EState ResultState => ResultSO.EState.LoseInQuest;

        protected override void HandleResult()
        {
            ActionDispatcher.Dispatch(new RestorePartyAction());
        }
    }
}