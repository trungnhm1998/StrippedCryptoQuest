using System.Collections.Generic;

namespace CryptoQuest.Gameplay.Battle.Core.Commands
{
    public interface ICommandHandler<T> where T : ICommand
    { 
        Queue<T> CommandsQueue { get; }

        void AddCommand(T command);

        void ExecuteCommand();
    }
}