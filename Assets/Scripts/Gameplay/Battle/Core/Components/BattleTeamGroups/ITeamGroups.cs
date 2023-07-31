using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;

namespace CryptoQuest.Gameplay.Battle.Core.Components
{
    public interface ITeamGroups
    {
        Dictionary<CharacterDataSO, int> GroupsDict { get; }
        BattleTeam Team { get; }

        void InitGroups();
        void RemoveUnitData(CharacterDataSO unit);
    }   

    public class NullTeamGroups : ITeamGroups
    {
        public Dictionary<CharacterDataSO, int> GroupsDict { get; } = new();
        public BattleTeam Team { get; }

        public void InitGroups() { }
        public void RemoveUnitData(CharacterDataSO unit) { }
    }   
}