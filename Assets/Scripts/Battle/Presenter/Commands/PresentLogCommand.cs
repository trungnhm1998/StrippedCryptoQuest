using System.Collections;
using CryptoQuest.Battle.UI.Logs;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.Presenter.Commands
{
    public class PresentLogCommand : IPresentCommand
    {
        private readonly LocalizedString _message;
        private readonly LogPresenter _presenter;
        private string _loadedMessage;

        public PresentLogCommand(LogPresenter presenter, LocalizedString message)
        {
            _presenter = presenter;
            _message = message;
        }

        public IEnumerator Load()
        {
            var handle = _message.GetLocalizedStringAsync();
            yield return handle;
            _loadedMessage = handle.Result;
        }

        public IEnumerator Present()
        {
            _presenter.Append(_loadedMessage);
            yield return new WaitForSeconds(_presenter.DelayBetweenLines);
        }
    }
}