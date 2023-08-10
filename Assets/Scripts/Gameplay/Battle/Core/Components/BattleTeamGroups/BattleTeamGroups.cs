using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components
{
    public class BattleTeamGroups : ITeamGroups
    {
        public Dictionary<int, int> GroupsDict { get; private set; } = new();
        public Dictionary<int, List<IBattleUnit>> UnitsDict { get; private set; } = new();
        public BattleTeam Team { get; private set; }
        private CharacterGroup[] _groupsData;

        public BattleTeamGroups(BattleTeam team, CharacterGroup[] groupsData)
        {
            Team = team;
            _groupsData = groupsData;
        }

        public void InitGroups()
        {
            HashSet<IBattleUnit> unitPool = new(Team.BattleUnits);
            for (int i = 0; i < _groupsData.Length; i++)
            {
                CharacterDataSO[] characters = _groupsData[i].Characters;
                if (characters.Length <= 0) continue;
                ExtractGroupUnits(i, characters, ref unitPool);
            }
        }

        private void ExtractGroupUnits(int groupIndex, CharacterDataSO[] characters,
            ref HashSet<IBattleUnit> currentUnsortedUnits)
        {
            List<IBattleUnit> unitInGroup = new(
                currentUnsortedUnits.Where(x => x.UnitData == characters[0]).Take(characters.Length));
            currentUnsortedUnits.RemoveWhere(x => unitInGroup.Contains(x));
            UnitsDict.Add(groupIndex, unitInGroup);
        }

        public void RemoveUnitData(IBattleUnit unit)
        {
            if (UnitsDict.Count <= 0) return;
            foreach (var unitPair in UnitsDict)
            {
                var units = unitPair.Value;
                if (!units.Contains(unit)) continue;
                units.Remove(unit);
            }
            
            RemoveEmptyGroup();
        }

        private void RemoveEmptyGroup()
        {
            var emptyGroups = UnitsDict.Where(u => u.Value.Count <= 0).ToList();
            foreach (var unitPair in emptyGroups)
            {
                UnitsDict.Remove(unitPair.Key);
            }
        }
    }
}