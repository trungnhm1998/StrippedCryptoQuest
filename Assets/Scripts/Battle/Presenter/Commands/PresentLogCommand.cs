using System.Collections;
using CryptoQuest.Battle.UI.Logs;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.Presenter.Commands
{
    public class PresentLogCommand : IPresentCommand
    {
        private readonly LogPresenter _presenter;
        private string _loadedMessage;

        public PresentLogCommand(LogPresenter presenter, LocalizedString message)
        {
            _presenter = presenter;
            message.GetLocalizedStringAsync().Completed += handle => _loadedMessage = handle.Result;
        }

        public IEnumerator Present()
        {
            _presenter.Append(_loadedMessage);
            yield return new WaitForSeconds(_presenter.DelayBetweenLines);
        }
    }
}