using CryptoQuest.Networking;
using CryptoQuest.Sagas.Profile;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.UI.Title.States
{
    public class CheckToAutoLoginState : MonoBehaviour, IState
    {
        [SerializeField] private Credentials _credentials;
        private TitleStateMachine _stateMachine;

        public void OnEnter(TitleStateMachine stateMachine)
        {
            _credentials.Load();
            _stateMachine = stateMachine;

            if (!_credentials.IsLoggedIn())
            {
                stateMachine.ChangeState(new TitleState());
                return;
            }

            TryToRefreshToken();
        }

        private void ToLogin()
        {
            _stateMachine.ChangeState(new TitleState());
        }

        private TinyMessageSubscriptionToken _refreshTokenFailed;

        private void TryToRefreshToken()
        {
            if (string.IsNullOrEmpty(_credentials.RefreshToken))
            {
                ToLogin();
                return;
            }

            _refreshTokenFailed = ActionDispatcher.Bind<RefreshTokenFailed>(_ => ToLogin());
            
            ActionDispatcher.Dispatch(new RefreshTokenAction());
        }

        public void OnExit(TitleStateMachine stateMachine)
        {
            if (_refreshTokenFailed != null) ActionDispatcher.Unbind(_refreshTokenFailed);
        }
    }
}