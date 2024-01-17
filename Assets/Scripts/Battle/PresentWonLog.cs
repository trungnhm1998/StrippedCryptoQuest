using UnityEngine.Localization;

namespace CryptoQuest.Battle
{
    public class PresentWonLog : PresentBattleResultLog
    {
        protected override ResultSO.EState State => ResultSO.EState.Win;
        protected override LocalizedString GetPrompt() => BattleBus.CurrentBattlefield.BattlefieldPrompts.WinPrompt;
    }
}