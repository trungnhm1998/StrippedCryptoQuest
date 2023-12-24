using CryptoQuest.Menu;
using CryptoQuest.Sagas;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Menus.Settings.UI
{
    public class UILogoutButton : MonoBehaviour
    {
        [field: SerializeField] public MultiInputButton Button { get; private set; }

        private TinyMessageSubscriptionToken _logoutFinished;

        private void OnEnable()
        {
            _logoutFinished = ActionDispatcher.Bind<LogoutFinishedAction>(OnLogoutFinished);
        }

        public void HandleLogoutButtonClicked()
        {
            ActionDispatcher.Dispatch(new LogoutAction());
        }

        private void OnLogoutFinished(LogoutFinishedAction _)
        {
            ActionDispatcher.Dispatch(new GoToTitleAction());
        }
    }
}
