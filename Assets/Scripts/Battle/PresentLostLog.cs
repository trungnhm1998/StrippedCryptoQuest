using UnityEngine.Localization;

namespace CryptoQuest.Battle
{
    public class PresentLostLog : PresentBattleResultLog
    {
        protected override ResultSO.EState State => ResultSO.EState.Lose;
        protected override LocalizedString GetPrompt() => BattleBus.CurrentBattlefield.BattlefieldPrompts.LosePrompt;
    }
}