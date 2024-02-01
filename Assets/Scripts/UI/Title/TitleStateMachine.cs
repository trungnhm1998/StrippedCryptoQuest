using System;
using System.Collections.Generic;
using CryptoQuest.Actions;
using CryptoQuest.Input;
using CryptoQuest.Networking;
using CryptoQuest.UI.Title.States;
using IndiGames.Core.Events;
using IndiGames.Core.Events.ScriptableObjects;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.UI.Title
{
    public interface IState
    {
        public void OnEnter(TitleStateMachine stateMachine);
        public void OnExit(TitleStateMachine stateMachine);
    }

    public class TitleStateMachine : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private VoidEventChannelSO _sceneLoadedEvent;
        [SerializeField] private CheckToAutoLoginState _checkLoginCheckState;
        [field: SerializeField] public GameObject LoginFailedPanel { get; private set; }
        [field: SerializeField] public float AutoCloseLoginFailedPanelTime { get; private set; }
        [field: SerializeField] public Credentials Credentials { get; private set; }

        private readonly Dictionary<Type, object> _cachedComponents = new();
        private IState _curState;
        private TinyMessageSubscriptionToken _getProfileSuccess;

        private void OnEnable()
        {
            _sceneLoadedEvent.EventRaised += ChangeToAutoLoginState;
            _getProfileSuccess = ActionDispatcher.Bind<GetProfileSucceed>(ToStartGame);
        }

        private void OnDisable()
        {
            _sceneLoadedEvent.EventRaised -= ChangeToAutoLoginState;
            ActionDispatcher.Unbind(_getProfileSuccess);
        }

        private void ToStartGame(GetProfileSucceed _)
        {
            ChangeState(new StartGameState());
        }

        private void ChangeToAutoLoginState() => ChangeState(_checkLoginCheckState);

        public void ChangeState(IState state)
        {
            _curState?.OnExit(this);
            _curState = state;
            _curState.OnEnter(this);
        }

        public new bool TryGetComponentInChildren<T>(out T component) where T : class
        {
            var type = typeof(T);
            if (_cachedComponents.TryGetValue(type, out var cachedComponent))
            {
                component = (T)cachedComponent;
                return true;
            }

            component = GetComponentInChildren<T>(true);
            var result = component != null;
            if (result)
                _cachedComponents.Add(type, component);

            return result;
        }
    }
}