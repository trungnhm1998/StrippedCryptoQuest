using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Indigames.AbilitySystem.Sample
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private List<AbilitySystemBehaviour> _battleTeam1;
        [SerializeField] private List<AbilitySystemBehaviour> _battleTeam2;

        [Header("Events")]
        [SerializeField] private VoidEventChannelSO _turnEndEventChannel;

        private List<IBattleUnit> _battleUnits = new();
        public List<IBattleUnit> BattleUnits => _battleUnits;

        private List<IBattleUnit> _pendingRemoveUnit = new();
        private int _turn = 0;


        private void Awake()
        {
            InitBattleUnits();
        }

        private void InitBattleUnits()
        {
            _battleUnits.Clear();
            InitBattleTeam(_battleTeam1, _battleTeam2);
            InitBattleTeam(_battleTeam2, _battleTeam1);
            // Might sort _battleUnits by speed or team 1 will execute first by default
        }

        private void InitBattleTeam(List<AbilitySystemBehaviour> team, List<AbilitySystemBehaviour> otherTeam)
        {
            foreach (var member in team)
            {
                var unit = member.gameObject.GetComponent<IBattleUnit>();
                unit.Init(this, member);
                unit.SetTeams(ref team, ref otherTeam);
                _battleUnits.Add(unit);
            }
        }

        public void RemoveUnit(IBattleUnit unit)
        {
            _pendingRemoveUnit.Add(unit);
        }

        public IEnumerator RemovePendingUnits()
        {
            while (_pendingRemoveUnit.Count > 0)
            {
                var unit = _pendingRemoveUnit[0];
                _pendingRemoveUnit.Remove(unit);
                RemoveUnitImmediately(unit);
                yield return true;
            }
        }
        
        public void RemoveUnitImmediately(IBattleUnit unit)
        {
            _battleUnits.Remove(unit);
            unit.OnDeath();
        }


        public bool IsBattleEnd()
        {
            return (_battleTeam1.Count <= 0) || (_battleTeam2.Count <= 0);
        }
    }   
}