using System;
using System.Collections;
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
        [SerializeField] private VoidEventChannelSO _onLoginFailedEventChannel;

        private void Awake()
        {
            ChangeState(new TitleState(this));
        }

        public void Subscribe()
        {
            _onLoginSuccesEventChannel.EventRaised += OnLoginSuccessEventChannel;
            _onLoginFailedEventChannel.EventRaised += OnLoginFailedEventChannel;
        }

        public void Unsubscribe()
        {
            _onLoginSuccesEventChannel.EventRaised -= OnLoginSuccessEventChannel;
            _onLoginFailedEventChannel.EventRaised -= OnLoginFailedEventChannel;
        }

        private void OnLoginSuccessEventChannel()
        {
            ChangeState(new StartGameState(StartGamePanelController));
        }

        private void OnLoginFailedEventChannel()
        {
            ChangeState(new SocialLoginFailedState(this));
        }

        public void ChangeState(IState state)
        {
            TitleStateMachine.ChangeState(state);
        }

        public void SelectDefault()
        {
            StartCoroutine(CoSelectDefault());
        }

        public IEnumerator CoSelectDefault()
        {
            yield return new WaitForSeconds(.5f);
            SocialPanel.TwitterLoginBtn.Select();
        }
    }
}