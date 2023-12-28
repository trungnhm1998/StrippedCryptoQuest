using System.Collections;
using CryptoQuest.Battle.UI.Logs;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.Presenter.Commands
{
    public class PresentLogCommand : IPresentCommand
    {
        private readonly LogPresenter _presenter;
        private LocalizedString _localizedMessage;

        public PresentLogCommand(LogPresenter presenter, LocalizedString message)
        {
            _presenter = presenter;
            _localizedMessage = message;
        }

        public IEnumerator Present()
        {
            var handle = _localizedMessage.GetLocalizedStringAsync();
            yield return handle;
            _presenter.Append(handle.Result);
            yield return new WaitForSeconds(_presenter.DelayBetweenLines);
        }
    }
}