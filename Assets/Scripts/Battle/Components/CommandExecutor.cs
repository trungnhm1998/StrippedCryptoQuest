using CryptoQuest.Battle.Commands;

namespace CryptoQuest.Battle.Components
{
    public interface ICommandExecutor { }

    public class CommandExecutor : CharacterComponentBase, ICommandExecutor
    {
        private ICommand _command;

        public ICommand Command
        {
            get => _command;
            protected set => _command = value;
        }

        public void SetCommand(ICommand command)
        {
            _command = command;
        }

        public void ExecuteCommand()
        {
            // this character could die during presentation phase
            if (Character.IsValidAndAlive() == false) return;
            OnPreExecuteCommand();
            _command.Execute(); // this should not be null
            OnPostExecuteCommand();
        }


        protected virtual void OnPreExecuteCommand() { }

        protected virtual void OnPostExecuteCommand()
        {
            _command = NullCommand.Instance;
        }

        public override void Init()
        {
            _command = NullCommand.Instance;
        }
    }
}