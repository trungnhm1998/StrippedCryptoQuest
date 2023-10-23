using System.Collections;
using CryptoQuest.Battle.UI.Logs;

namespace CryptoQuest.Battle.Presenter.Commands
{
    public class HideDialog : IPresentCommand
    {
        private LogPresenter _logPresenter;

        public HideDialog(LogPresenter logPresenter)
        {
            _logPresenter = logPresenter;
        }

        public IEnumerator Present()
        {
            _logPresenter.HideAndClear();
            yield break;
        }
    }
}