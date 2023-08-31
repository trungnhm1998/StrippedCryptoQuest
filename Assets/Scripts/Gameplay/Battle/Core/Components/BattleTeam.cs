using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components
{
    [Obsolete]
    public class BattleTeam : MonoBehaviour
    {
        [field: SerializeField]
        public List<AbilitySystemBehaviour> Members { get; private set; }

        public List<IBattleUnit> BattleUnits { get; private set; } = new();
        public ITeamGroups TeamGroups { get; set; } = new NullTeamGroups();

        private List<IBattleUnit> _pendingRemoveUnit = new();

        public void InitBattleUnits(BattleTeam opponentTeam)
        {
            BattleUnits.Clear();

            foreach (var member in Members)
            {
                if (!member.gameObject.activeSelf) continue;
                var unit = member.gameObject.GetComponent<IBattleUnit>();
                unit.Init(this, member);
                unit.SetOpponentTeams(opponentTeam);
                BattleUnits.Add(unit);
            }

            TeamGroups.InitGroups();
        }

        public void RemoveUnit(IBattleUnit unit)
        {
            TeamGroups.RemoveUnitData(unit);
            BattleUnits.Remove(unit);
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

        public IEnumerable<AbilitySystemBehaviour> GetAvailableMembers()
        {
            foreach (var battleUnit in BattleUnits)
            {
                yield return battleUnit.Owner;
            }
        }

        public bool IsWiped() => BattleUnits.Count <= 0;
    }
}