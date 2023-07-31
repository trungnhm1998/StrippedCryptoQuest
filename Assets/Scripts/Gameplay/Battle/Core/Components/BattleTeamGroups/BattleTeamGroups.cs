using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components
{
    public class BattleTeamGroups : ITeamGroups
    {
        public Dictionary<CharacterDataSO, int> GroupsDict { get; private set; } = new();
        public BattleTeam Team { get; private set; }
        private CharacterGroup[] _groupsData;

        public BattleTeamGroups(BattleTeam team, CharacterGroup[] groupsData)
        {
            Team = team;
            _groupsData = groupsData;
        }

        public void InitGroups()
        {
            foreach (var group in _groupsData)
            {
                CharacterDataSO[] characters = group.Characters;
                if (characters.Length <= 0) continue;
                CharacterDataSO characterData = characters[0];
                GroupsDict.Add(characterData, characters.Length);
            }
        }

        public void RemoveUnitData(CharacterDataSO data)
        {
            if (!GroupsDict.ContainsKey(data)) return;
            GroupsDict[data]--;
            if (GroupsDict[data] > 0) return;
            GroupsDict.Remove(data);
        }
    }   
}