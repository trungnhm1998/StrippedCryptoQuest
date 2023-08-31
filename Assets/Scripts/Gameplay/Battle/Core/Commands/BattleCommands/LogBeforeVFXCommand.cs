using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;

namespace CryptoQuest.Gameplay.Battle.Core.Commands.BattleCommands
{
    /// <summary>
    /// Log will be showed first then VFX can perform and finish command
    /// </summary>
    public class LogBeforeVFXCommand : BattleCommand
    {
        public LogBeforeVFXCommand(BattleActionDataSO data) : base(data) { }

        protected override void VFXFinished()
        {
            FinishedCommand?.Invoke();
        }
        
        protected override void ShowLog()
        {
            base.ShowLog();
            LoadVFX();
        }

        public override void Execute()
        {
            ShowLog();
        }
    }
}