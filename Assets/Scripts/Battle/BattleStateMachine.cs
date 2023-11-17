using System;
using System.Collections.Generic;
using CryptoQuest.Battle.States;
using CryptoQuest.Battle.UI.SelectCommand;
using CryptoQuest.Battle.UI.SelectHero;
using CryptoQuest.Battle.UI.StartBattle;
using CryptoQuest.Input;
using CryptoQuest.UI.SpiralFX;
using IndiGames.Core.Events.ScriptableObjects;
using Input;
using UnityEngine;

namespace CryptoQuest.Battle
{
    public interface IState
    {
        void OnEnter(BattleStateMachine stateMachine);
        void OnExit(BattleStateMachine stateMachine);
    }

    public class BattleStateMachine : MonoBehaviour
    {
        [field: SerializeField] public BattleInput BattleInput { get; private set; }

        private readonly Dictionary<Type, Component> _cachedComponents = new();

        #region State Context

        [field: SerializeField] public GameObject BattleUI { get; private set; }
        [field: SerializeField] public UIIntroBattle IntroUI { get; private set; }
        [field: SerializeField] public UISelectCommand CommandUI { get; private set; }
        [field: SerializeField] public SpiralConfigSO Spiral { get; private set; }
        [field: SerializeField] public TransitionEventChannelSO TransitionEventChannelSo { get; private set; }
        [field: SerializeField] public AbstractTransition TransitionOut { get; private set; }
        [field: SerializeField] public VoidEventChannelSO SceneLoadedEvent { get; private set; }
        [field: SerializeField] public SelectHeroPresenter SelectHeroPresenter { get; private set; }

        #endregion

        #region State management
        public IState HandleActions { get; private set; } // I want this state to have it owns state-state

        private IState _currentState;

        private void Awake()
        {
            HandleActions = new ExecuteCharactersActions();
            SceneLoadedEvent.EventRaised += GotoLoadingState;
        }

        /// <summary>
        /// I use OnDisable to prevent error in editor
        /// because when state Exit it only disabling GO and those GO already destroyed 
        /// </summary>
        private void OnDisable() => Unload();

        public void Unload()
        {
            SceneLoadedEvent.EventRaised -= GotoLoadingState;
            _currentState?.OnExit(this);
            _currentState = null;
            _cachedComponents.Clear();
        }

        private void GotoLoadingState()
        {
            ChangeState(new Loading());
        }

        public void ChangeState(IState state)
        {
            Debug.Log($"BattleStateMachine: ChangeState {state.GetType().Name}");
            _currentState?.OnExit(this);
            _currentState = state;
            _currentState.OnEnter(this);
        }

        /// <summary>
        /// Same as Unity's <see cref="GameObject.TryGetComponent{T}(out T)"/> but with a cache
        /// </summary>
        public new bool TryGetComponent<T>(out T component) where T : Component
        {
            var type = typeof(T);
            if (!_cachedComponents.TryGetValue(type, out var value))
            {
                if (base.TryGetComponent(out component))
                    _cachedComponents.Add(type, component);

                return component != null;
            }

            component = (T)value;
            return true;
        }

        #endregion
    }
}