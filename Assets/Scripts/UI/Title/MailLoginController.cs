using CryptoQuest.Events;
using CryptoQuest.UI.Title.TitleStates;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Title
{
    public class MailLoginController : MonoBehaviour
    {
        [field: SerializeField] public TitlePanelController TitlePanelController { get; private set; }
        [field: SerializeField] public StartGamePanelController StartGamePanelController { get; set; }
        [field: SerializeField] public UISignInPanel UISignInPanel { get; private set; }
        [SerializeField] private AuthenticateFormInfoEventChannelSO _onRequestLoginEventChannel;
        [SerializeField] private VoidEventChannelSO _onLoginSuccesEventChannel;
        [SerializeField] private StringEventChannelSO _onLoginFailedEventChannel;

        public void Subscribe()
        {
            _onLoginSuccesEventChannel.EventRaised += OnLoginSuccess;
            _onLoginFailedEventChannel.EventRaised += OnLoginFailed;
        }

        public void Unsubscribe()
        {
            _onLoginSuccesEventChannel.EventRaised -= OnLoginSuccess;
            _onLoginFailedEventChannel.EventRaised -= OnLoginFailed;
        }

        public void OnLoginFormSubmit()
        {
            _onRequestLoginEventChannel.RaiseEvent(UISignInPanel.GetFormInfo());
        }

        public void ChangeState(IState state)
        {
            TitlePanelController.TitleStateMachine.ChangeState(state);
        }

        private void OnLoginSuccess()
        {
            ChangeState(new StartGameState(StartGamePanelController));
        }

        public void OnMailLoginPressed()
        {
            ChangeState(new MailLoginState(this));
        }

        private void OnLoginFailed(string error)
        {
            ChangeState(new LoginFormFailedState(this));
        }
    }
}