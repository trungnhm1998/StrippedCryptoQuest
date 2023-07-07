using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using CryptoQuest.FSM;

namespace CryptoQuest.Gameplay.Battle
{
    public class BattleTeam : MonoBehaviour
    {
        [SerializeField] private List<AbilitySystemBehaviour> _members;
        public List<AbilitySystemBehaviour> Members => _members;

        private List<IBattleUnit> _battleUnits = new();
        public List<IBattleUnit> BattleUnits => _battleUnits;
        private List<IBattleUnit> _pendingRemoveUnit = new();

        public void InitBattleUnits(BattleTeam opponentTeam)
        {
            _battleUnits.Clear();

            foreach (var member in _members)
            {
                var unit = member.gameObject.GetComponent<IBattleUnit>();
                unit.Init(this, member);
                unit.SetOpponentTeams(opponentTeam);
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

        public bool IsWiped()
        {
            return _battleUnits.Count <= 0;
        }
    }   
}