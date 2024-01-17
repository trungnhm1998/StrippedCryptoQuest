using System;
using System.Collections;
using CryptoQuest.Battle.Presenter.Commands;

namespace CryptoQuest.Battle.Presenter
{
    public class ResultLogPresenter : IPresentCommand
    {
        private Func<IEnumerator> _coShowLog;

        public ResultLogPresenter(Func<IEnumerator> coShowLog)
        {
            _coShowLog = coShowLog;
        }

        public IEnumerator Present()
        {
            yield return _coShowLog();
        }
    }
}