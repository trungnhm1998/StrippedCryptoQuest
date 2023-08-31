using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;

namespace CryptoQuest.Gameplay.Battle.Core.Commands.BattleCommands
{
    
    /// <summary>
    /// The command finish right after showing log
    /// </summary>
    public class LogOnlyCommand : BattleCommand
    {
        public LogOnlyCommand(BattleActionDataSO data) : base(data) { }

        public override void Execute()
        {
            ShowLog();
            FinishedCommand?.Invoke();
        }
    }
}