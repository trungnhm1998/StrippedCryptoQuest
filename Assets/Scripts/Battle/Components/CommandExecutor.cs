using System;
using CryptoQuest.Battle.Commands;

namespace CryptoQuest.Battle.Components
{
    public class CommandExecutor : CharacterComponentBase
    {
        public event Action PreExecuteCommand;
        public event Action PostExecuteCommand;
        public ICommand Command { get; private set; }

        public void SetCommand(ICommand command)
        {
            Command = command;
        }

        public void ExecuteCommand()
        {
            // this character could die during presentation phase
            if (Character.IsValidAndAlive() == false) return;
            PreExecuteCommand?.Invoke();
            Command.Execute(); // this should not be null
            PostExecuteCommand?.Invoke();
            Command = NullCommand.Instance;
        }

        protected override void OnInit()
        {
            Command = NullCommand.Instance;
        }
    }
}