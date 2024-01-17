using System;

namespace CryptoQuest.Battle.PostBattleHandlers
{
    public class EndBattleHandler : PostBattleManager
    {
        protected override ResultSO.EState ResultState => ResultSO.EState.Retreat;
    }
}