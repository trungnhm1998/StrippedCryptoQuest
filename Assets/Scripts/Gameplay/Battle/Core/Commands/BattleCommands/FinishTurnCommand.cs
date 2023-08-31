using System;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.Gameplay.Battle.Core.Commands.BattleCommands
{
    public class FinishTurnCommand : BattleCommand
    {
        public Action ShowNextMarkEvent;
        public VoidEventChannelSO DoneShowDialogEvent;
        
        public FinishTurnCommand(BattleActionDataSO data = null) : base(data) { }

        public override void Execute()
        {
            DoneShowDialogEvent.EventRaised += FinishedCommand.Invoke;
            ShowNextMarkEvent?.Invoke();
        }

        protected override void CleanUp()
        {
            DoneShowDialogEvent.EventRaised -= FinishedCommand.Invoke;
            base.CleanUp();
        }
    }
}