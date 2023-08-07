using System.Collections;
using System.Collections.Generic;
using CryptoQuest.FSM;
using CryptoQuest.FSM.ScriptableObjects;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleSpawner;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleOrder;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CryptoQuest.Gameplay.Battle.Core.Components
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private BaseStateMachine _stateMachine;
        [SerializeField] private StateSO _battleStartState;
        [SerializeField] private StateSO _battleEndState;
        [SerializeField] private BattleBus _battleBus;
        [SerializeField] private BattleOrderDecider _battleOrderDecider;

        [field: SerializeField] public BattleTeam BattleTeam1 { get; protected set; }

        [field: SerializeField] public BattleTeam BattleTeam2 { get; protected set; }

        [Header("Raise Events")] [SerializeField]
        private VoidEventChannelSO _newTurnEventChannel;

        [Header("Listen Events")] [SerializeField]
        private VoidEventChannelSO _startBattleEventChannel;

        [SerializeField] private VoidEventChannelSO _endActionPhaseEventChannel;
        [SerializeField] private SpecialAbilitySO _retreatAbility;
        [SerializeField] private VoidEventChannelSO _onBattleEndEventChannel;
        public IBattleUnit CurrentUnit { get; set; } = NullBattleUnit.Instance;
        public int Turn { get; private set; }
        public BaseBattleSpawner BattleSpawner { get; private set; }
        public List<IBattleUnit> BattleUnits { get; private set; } = new();
        public bool IsEndTurn { get; private set; }
        public BattleInfo CurrentBattleInfo { get; private set; }

        protected void Awake()
        {
            BattleSpawner = GetComponent<BaseBattleSpawner>();
            _battleBus.BattleManager = this;
            CurrentBattleInfo = _battleBus.CurrentBattleInfo;
        }

        protected virtual void StartBattle()
        {
            _stateMachine.SetCurrentState(_battleStartState);
        }

        public void InitBattle()
        {
            BattleSpawner.SpawnBattle(CurrentBattleInfo.BattleDataSO);
        }

        private void OnEnable()
        {
            _endActionPhaseEventChannel.EventRaised += OnEndTurn;
            _startBattleEventChannel.EventRaised += StartBattle;
            _retreatAbility.OnAbilityActivated += OnRetreatActivated;
        }

        private void OnDisable()
        {
            _endActionPhaseEventChannel.EventRaised -= OnEndTurn;
            _startBattleEventChannel.EventRaised -= StartBattle;
            _retreatAbility.OnAbilityActivated -= OnRetreatActivated;
        }

#if UNITY_EDITOR
        private void OnDestroy()
        {
            // To make sure when battle manager is detroyed, the battle end and
            // the global state is reset, to prevent error when cold boot
            if (_stateMachine.CurrentState != _battleEndState)
            {
                OnRetreat();
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

        private void OnRetreatActivated(AbilityScriptableObject abilityScriptableObject)
        {
            OnRetreat();
        }

        public void OnRetreat()
        {
            _stateMachine.SetCurrentState(_battleEndState);
        }

        public void OnBattleEnd()
        {
            _onBattleEndEventChannel.RaiseEvent();
        }

        public List<IBattleUnit> GetActionOrderList()
        {
            return _battleOrderDecider.SortUnitByAttributeValue(BattleUnits);
        }
    }

    public struct BattleInfo
    {
        public BattleDataSO BattleDataSO;
        public bool IsBattleEscapable;
        public AssetReferenceT<Sprite> BattleBackground;

        public BattleInfo(BattleDataSO battleDataSo, bool isBattleEscapable,
            AssetReferenceT<Sprite> battleBackground = null)
        {
            BattleDataSO = battleDataSo;
            IsBattleEscapable = isBattleEscapable;
            BattleBackground = battleBackground;
        }
    }
}