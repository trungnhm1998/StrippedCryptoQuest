using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;

namespace CryptoQuest.Gameplay.Battle.Core.Commands.BattleCommands
{
    /// <summary>
    /// Log will be showed after VFX
    /// </summary>
    public class LogAfterVFXCommand : BattleCommand
    {
        public LogAfterVFXCommand(BattleActionDataSO data) : base(data) { }

        protected override void VFXFinished()
        {
            ShowLog();
            FinishedCommand?.Invoke();
        }

        public override void Execute()
        {
            LoadVFX();
        }
    }
}