using CryptoQuest.Battle.UI.Logs;

namespace CryptoQuest.Battle.Presenter
{
    public class ClearLog : IPresentCommand
    {
        private readonly LogPresenter _logPresenter;

        public ClearLog(LogPresenter logPresenter) => _logPresenter = logPresenter;

        public StateBase GetState() => new ClearLogState(_logPresenter);
    }

    public class ClearLogState : StateBase
    {
        private readonly LogPresenter _logPresenter;

        public ClearLogState(LogPresenter logPresenter)
        {
            _logPresenter = logPresenter;
        }

        protected override void OnEnter()
        {
            _logPresenter.Clear();
            StateMachine.ChangeState(StateMachine.GetNextCommand);
        }
    }
}