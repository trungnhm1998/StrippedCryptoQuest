using UnityEngine;

namespace CryptoQuest.Battle.UI.Logs
{
    public abstract class LoggerComponentBase : MonoBehaviour
    {
        protected LogPresenter Logger { get; private set; }

        public void Init(LogPresenter logPresenter) => Logger = logPresenter;
    }
}