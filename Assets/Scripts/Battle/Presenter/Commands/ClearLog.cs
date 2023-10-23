using System.Collections;
using CryptoQuest.Battle.UI.Logs;

namespace CryptoQuest.Battle.Presenter.Commands
{
    public class ClearLog : IPresentCommand
    {
        private readonly LogPresenter _logPresenter;

        public ClearLog(LogPresenter logPresenter) => _logPresenter = logPresenter;

        public IEnumerator Present()
        {
            _logPresenter.Clear();
            yield break;
        }
    }
}