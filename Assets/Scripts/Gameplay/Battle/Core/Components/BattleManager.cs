using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using CryptoQuest.FSM;

namespace CryptoQuest.Gameplay.Battle
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private BaseStateMachine _stateMachine;
        [SerializeField] private StateSO _battleEndState;

        [field: SerializeField]
        public BattleTeam BattleTeam1 { get; private set; }
        [field: SerializeField]
        public BattleTeam BattleTeam2 { get; private set; }

        public IBattleUnit CurrentUnit {get; set;}
        public int Turn { get; private set; }
        public BaseBattleSpawner BattleSpawner { get; private set; }
        public List<IBattleUnit> BattleUnits { get; private set; } = new();

        protected void Awake()
        {
            BattleSpawner = GetComponent<BaseBattleSpawner>();
            InitBattleTeams();
        }

        public void InitBattleTeams()
        {
            BattleUnits.Clear();
            BattleTeam1.InitBattleUnits(BattleTeam2);
            BattleUnits.AddRange(BattleTeam1.BattleUnits);
            BattleTeam2.InitBattleUnits(BattleTeam1);
            BattleUnits.AddRange(BattleTeam2.BattleUnits);
        }

        public IEnumerator RemovePendingUnits()
        {
            yield return BattleTeam1.RemovePendingUnits();
            yield return BattleTeam2.RemovePendingUnits();

            for (int i = 0; i < BattleUnits.Count; i++)
            {
                IBattleUnit unit = BattleUnits[i];
                if (!unit.IsDead) continue;
                
                BattleUnits.Remove(unit);
                i--;
            }
        }

        public bool IsBattleEnd()
        {
            return (BattleTeam1.IsWiped() || BattleTeam2.IsWiped());
        }

        public void OnNewTurn()
        {
            Turn++;
        }

        public void OnEscape()
        {
            _stateMachine.SetCurrentState(_battleEndState);
        }
    }   
}