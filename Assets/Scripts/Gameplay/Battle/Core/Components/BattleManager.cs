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

        [SerializeField] private BattleTeam _battleTeam1;
        public BattleTeam BattleTeam1 => _battleTeam1;
        [SerializeField] private BattleTeam _battleTeam2;
        public BattleTeam BattleTeam2 => _battleTeam2;

        public IBattleUnit CurrentUnit {get; set;}
        private int _turn = 0;
        public int Turn => _turn;
        private BaseBattleSpawner _battleSpawner;
        public BaseBattleSpawner BattleSpawner => _battleSpawner;
        private List<IBattleUnit> _battleUnits = new();
        public List<IBattleUnit> BattleUnits => _battleUnits;

        protected void Awake()
        {
            _battleSpawner = GetComponent<BaseBattleSpawner>();
            InitBattleTeams();
        }

        public void InitBattleTeams()
        {
            _battleUnits.Clear();
            _battleTeam1.InitBattleUnits(_battleTeam2);
            _battleUnits.AddRange(_battleTeam1.BattleUnits);
            _battleTeam2.InitBattleUnits(_battleTeam1);
            _battleUnits.AddRange(_battleTeam2.BattleUnits);
        }


        public IEnumerator RemovePendingUnits()
        {
            yield return _battleTeam1.RemovePendingUnits();
            yield return _battleTeam2.RemovePendingUnits();
        }

        public bool IsBattleEnd()
        {
            return (_battleTeam1.IsWiped() || _battleTeam2.IsWiped());
        }

        public void OnNewTurn()
        {
            _turn++;
        }

        public void OnEscape()
        {
            _stateMachine.SetCurrentState(_battleEndState);
        }
    }   
}