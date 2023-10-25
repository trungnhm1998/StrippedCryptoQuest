
using System.Collections;
using CryptoQuest.Battle.States;

namespace CryptoQuest.Battle.Presenter.Commands
{
    public class RetreatSuccessCommand : IPresentCommand
    {
        private ExecuteCharactersActions _executeAction;

        public RetreatSuccessCommand(ExecuteCharactersActions executeAction)
        {
            _executeAction = executeAction;
        }

        public IEnumerator Present()
        {
            _executeAction.OnEndBattle();
            yield break;
        }
    }
}