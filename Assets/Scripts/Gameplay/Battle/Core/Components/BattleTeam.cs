using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using CryptoQuest.FSM;

namespace CryptoQuest.Gameplay.Battle
{
    public class BattleTeam : MonoBehaviour
    {
        [field: SerializeField]
        public List<AbilitySystemBehaviour> Members { get; private set; }

        public List<IBattleUnit> BattleUnits { get; private set; } = new();

        private List<IBattleUnit> _pendingRemoveUnit = new();

        public void InitBattleUnits(BattleTeam opponentTeam)
        {
            BattleUnits.Clear();

            foreach (var member in Members)
            {
                var unit = member.gameObject.GetComponent<IBattleUnit>();
                unit.Init(this, member);
                unit.SetOpponentTeams(opponentTeam);
                BattleUnits.Add(unit);
            }
        }

        public void RemoveUnit(IBattleUnit unit)
        {
            BattleUnits.Remove(unit);
            Members.Remove(unit.Owner);
            _pendingRemoveUnit.Add(unit);
        }

        public IEnumerator RemovePendingUnits()
        {
            while (_pendingRemoveUnit.Count > 0)
            {
                IBattleUnit unit = _pendingRemoveUnit[0];
                _pendingRemoveUnit.Remove(unit);
                unit.OnDeath();
                yield return null;
            }
        }

        public bool IsWiped() => BattleUnits.Count <= 0;
    }   
}