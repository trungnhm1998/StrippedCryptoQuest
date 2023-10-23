using System.Collections;
using CryptoQuest.Battle.UI.Logs;
using UnityEngine;

namespace CryptoQuest.Battle.Presenter.Commands
{
    public class ShowDialog : IPresentCommand
    {
        private readonly LogPresenter _logPresenter;
        private readonly int _round;

        public ShowDialog(LogPresenter logPresenter, int round)
        {
            _round = round;
            _logPresenter = logPresenter;
        }

        public IEnumerator Present()
        {
            Debug.Log($"Show dialog round[{_round}]");
            _logPresenter.Show();
            yield break;
        }
    }
}