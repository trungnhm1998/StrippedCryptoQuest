using UnityEngine;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
    using System.Linq;
    using System.IO;
    using UnityEditor;
#endif

namespace CryptoQuest.FSM
{
    public class BaseStateMachine : MonoBehaviour
    {
        [SerializeField] private BaseStateSO _initialState;
        private Dictionary<Type, Component> _cachedComponents = new();
        
        public BaseStateSO CurrentState { get; private set; }

#if UNITY_EDITOR
        public static NullStateSO LoadNullStateSO()
        {
            return AssetDatabase.FindAssets($"t:{typeof(NullStateSO).Name} ")
                .Select(x => AssetDatabase.GUIDToAssetPath(x))
                .SelectMany(x => AssetDatabase.LoadAllAssetsAtPath(x))
                .OfType<NullStateSO>().FirstOrDefault();
        }

        private void OnValidate()
        {
            if (_initialState != null) return;
            _initialState = LoadNullStateSO();
        }
#endif

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

            if (TryGetComponent<T>(out var notCachedComponent))
            {
                _cachedComponents[typeof(T)] = notCachedComponent;
            }
            return notCachedComponent;
        }
    }
}