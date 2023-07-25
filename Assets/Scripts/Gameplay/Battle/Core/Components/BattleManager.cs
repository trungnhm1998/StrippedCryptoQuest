using System.Collections;
using System.Collections.Generic;
using CryptoQuest.FSM;
using CryptoQuest.FSM.ScriptableObjects;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleSpawner;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private BaseStateMachine _stateMachine;
        [SerializeField] private StateSO _battleStartState;
        [SerializeField] private StateSO _battleEndState;
        [SerializeField] private BattleBus _battleBus;

        [field: SerializeField]
        public BattleTeam BattleTeam1 { get; protected set; }
        [field: SerializeField]
        public BattleTeam BattleTeam2 { get; protected set; }

        [Header("Raise Events")]
        [SerializeField] private VoidEventChannelSO _newTurnEventChannel;

        [Header("Listen Events")]
        [SerializeField] private VoidEventChannelSO _onDialogCloseEventChannel;
        [SerializeField] private VoidEventChannelSO _sceneLoadedEventChannel;

        public IBattleUnit CurrentUnit {get; set;}
        public int Turn { get; private set; }
        public BaseBattleSpawner BattleSpawner { get; private set; }
        public List<IBattleUnit> BattleUnits { get; private set; } = new();

        public bool IsEndTurn { get; private set; }

        protected void Awake()
        {
            BattleSpawner = GetComponent<BaseBattleSpawner>();
            _battleBus.BattleManager = this;
        }

        protected virtual void StartBattle()
        {
            _stateMachine.SetCurrentState(_battleStartState);
        }

        private void OnEnable()
        {
            _onDialogCloseEventChannel.EventRaised += OnEndTurn;
            _sceneLoadedEventChannel.EventRaised += StartBattle;
        }

        private void OnDisable()
        {
            _onDialogCloseEventChannel.EventRaised -= OnEndTurn;
            _sceneLoadedEventChannel.EventRaised -= StartBattle;
        }

#if UNITY_EDITOR
        private void OnDestroy()
        {
            // To make sure when battle manager is detroyed, the battle end and
            // the global state is reset, to prevent error when cold boot
            if (_stateMachine.CurrentState != _battleEndState)
            {
                OnEscape();
            }
        }
#endif 

        public void InitBattleTeams()
        {
            BattleUnits.Clear();
            SetupTeam1();
            SetupTeam2();
        }

        private void SetupTeam1()
        {
            BattleTeam1.InitBattleUnits(BattleTeam2);
            BattleUnits.AddRange(BattleTeam1.BattleUnits);
        }

        private void SetupTeam2()
        {
            BattleTeam2.InitBattleUnits(BattleTeam1);
            BattleUnits.AddRange(BattleTeam2.BattleUnits);
        }

        public IEnumerator RemovePendingUnits()
        {
            yield return BattleTeam1.RemovePendingUnits();
            yield return BattleTeam2.RemovePendingUnits();

            for (int i = BattleUnits.Count - 1; i >= 0; i--)
            {
                IBattleUnit unit = BattleUnits[i];
                if (!unit.IsDead) continue;
                
                BattleUnits.Remove(unit);
            }
        }

        public bool IsBattleEnd()
        {
            return (BattleTeam1.IsWiped() || BattleTeam2.IsWiped());
        }

        public void OnNewTurn()
        {
            IsEndTurn = false;
            _newTurnEventChannel.RaiseEvent();
            Turn++;
        }

        private void OnEndTurn()
        {
            IsEndTurn = true;
        }
        
        public void OnEscape()
        {
            _stateMachine.SetCurrentState(_battleEndState);
        }
    }   
}