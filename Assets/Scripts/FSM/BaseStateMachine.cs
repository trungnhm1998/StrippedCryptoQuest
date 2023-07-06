using UnityEngine;
using System.Collections.Generic;
using System;

namespace CryptoQuest.FSM
{
    public class BaseStateMachine : MonoBehaviour
    {
        [SerializeField] private BaseStateSO _initialState;
        private Dictionary<Type, Component> _cachedComponents = new();
        
        public BaseStateSO CurrentState { get; set; }

        private void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            SetCurrentState(_initialState);
        }

        private void Update()
        {
            Execute();
        }
        
        protected virtual void Execute()
        {
            CurrentState.Execute(this);
        }

        public void SetCurrentState(BaseStateSO nextState)
        {
            if (CurrentState != null)
            {
                CurrentState.OnExitState(this);
            }
            CurrentState = nextState;
            CurrentState.OnEnterState(this);
        }

        public new T GetComponent<T>() where T : Component
        {
            if (_cachedComponents.TryGetValue(typeof(T), out var component))
            {
                return component as T;
            }

            component = base.GetComponent<T>();
            if (component != null)
            {
                _cachedComponents.Add(typeof(T), component);
            }
            return component as T;
        }
    }
}