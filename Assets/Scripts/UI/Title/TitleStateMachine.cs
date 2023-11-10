using System;
using System.Collections.Generic;
using CryptoQuest.Input;
using CryptoQuest.UI.Title.States;
using IndiGames.Core.Events.ScriptableObjects;
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
        [field: SerializeField] public GameObject LoginFailedPanel { get; private set; }
        [field: SerializeField] public float AutoCloseLoginFailedPanelTime { get; private set; }

        private readonly Dictionary<Type, object> _cachedComponents = new();
        private IState _curState;

        private void OnEnable() => _sceneLoadedEvent.EventRaised += ChangeToAutoLoginState;

        private void OnDisable() => _sceneLoadedEvent.EventRaised -= ChangeToAutoLoginState;

        private void ChangeToAutoLoginState() => ChangeState(new AutoLoginState());

        public void ChangeState(IState state)
        {
            if (_curState != null)
                _curState.OnExit(this);

            _curState = state;
            _curState.OnEnter(this);
        }

        public new bool TryGetComponent<T>(out T component) where T : class
        {
            var type = typeof(T);
            if (_cachedComponents.TryGetValue(type, out var cachedComponent))
            {
                component = (T)cachedComponent;
                return true;
            }

            var result = base.TryGetComponent(out component);
            if (result)
                _cachedComponents.Add(type, component);

            return result;
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