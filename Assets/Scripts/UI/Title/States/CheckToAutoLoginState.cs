using CryptoQuest.Actions;
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
        private string _token;
        private string _refreshToken;
        private TitleStateMachine _stateMachine;
        private TinyMessageSubscriptionToken _fetchFailed;
        private TinyMessageSubscriptionToken _fetchSucceed;

        public void OnEnter(TitleStateMachine stateMachine)
        {
            _credentials.Load();
            _stateMachine = stateMachine;

            if (!_credentials.IsLoggedIn())
            {
                stateMachine.ChangeState(new TitleState());
                return;
            }

            _fetchFailed = ActionDispatcher.Bind<FetchProfileFailedAction>(TryToRefreshToken);
            _fetchSucceed = ActionDispatcher.Bind<FetchProfileSucceedAction>(_ =>
            {
                if (_fetchSucceed != null) ActionDispatcher.Unbind(_fetchSucceed);
                ActionDispatcher.Dispatch(new AuthenticateSucceed());
            });

            ActionDispatcher.Dispatch(new FetchProfileAction());
        }

        private void ToLogin()
        {
            _stateMachine.ChangeState(new TitleState());
        }

        private TinyMessageSubscriptionToken _refreshTokenFailed;

        private void TryToRefreshToken(FetchProfileFailedAction ctx)
        {
            if (_fetchFailed != null) ActionDispatcher.Unbind(_fetchFailed);
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
            if (_fetchFailed != null) ActionDispatcher.Unbind(_fetchFailed);
            if (_fetchSucceed != null) ActionDispatcher.Unbind(_fetchSucceed);
            if (_refreshTokenFailed != null) ActionDispatcher.Unbind(_refreshTokenFailed);
        }
    }
}