using CryptoQuest.Events;
using CryptoQuest.UI.Title.TitleStates;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Title
{
    public class TitlePanelController : MonoBehaviour
    {
        [field: SerializeField] public TitleStateMachine TitleStateMachine { get; private set; }
        [field: SerializeField] public StartGamePanelController StartGamePanelController { get; private set; }
        [field: SerializeField] public UISocialPanel SocialPanel { get; private set; }
        [SerializeField] private VoidEventChannelSO _onLoginSuccesEventChannel;
        [SerializeField] private StringEventChannelSO _onLoginFailedEventChannel;

        private void Awake() => ChangeState(new TitleState(this));

        public void OnEnable()
        {
            _onLoginSuccesEventChannel.EventRaised += OnLoginSuccessEventChannel;
            _onLoginFailedEventChannel.EventRaised += OnLoginFailedEventChannel;
        }

        public void OnDisable()
        {
            _onLoginSuccesEventChannel.EventRaised -= OnLoginSuccessEventChannel;
            _onLoginFailedEventChannel.EventRaised -= OnLoginFailedEventChannel;
        }

        private void OnLoginSuccessEventChannel() => ChangeState(new StartGameState(StartGamePanelController));

        private void OnLoginFailedEventChannel(string error) => ChangeState(new SocialLoginFailedState(this));

        public void ChangeState(IState state) => TitleStateMachine.ChangeState(state);

        public void SelectDefault() => Invoke(nameof(SelectTwitterButton), 0.5f);

        private void SelectTwitterButton() => SocialPanel.TwitterLoginBtn.Select();
    }
}