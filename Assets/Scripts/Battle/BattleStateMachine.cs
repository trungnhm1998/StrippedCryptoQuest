using System;
using System.Collections.Generic;
using CryptoQuest.Battle.States;
using CryptoQuest.Battle.UI.SelectCommand;
using CryptoQuest.Battle.UI.SelectHero;
using CryptoQuest.Battle.UI.StartBattle;
using CryptoQuest.Input;
using CryptoQuest.UI.SpiralFX;
using IndiGames.Core.Events.ScriptableObjects;
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

        #region State Context

        [field: SerializeField] public BattlePresenter BattlePresenter { get; private set; }
        [field: SerializeField] public VoidEventChannelSO SceneLoadedEvent { get; private set; }

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

        public bool TryGetPresenterComponent<T>(out T component) where T : Component
            => BattlePresenter.TryGetComponent(out component);

        #endregion
    }
}