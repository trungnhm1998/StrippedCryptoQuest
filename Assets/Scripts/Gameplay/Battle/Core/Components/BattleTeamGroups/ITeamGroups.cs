using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;

namespace CryptoQuest.Gameplay.Battle.Core.Components
{
    public interface ITeamGroups
    {
        Dictionary<int, int> GroupsDict { get; }
        Dictionary<int, List<IBattleUnit>> UnitsDict { get; }
        BattleTeam Team { get; }

        void InitGroups();
        void RemoveUnitData(IBattleUnit unit);
    }

    public class NullTeamGroups : ITeamGroups
    {
        public Dictionary<int, int> GroupsDict { get; } = new();
        public Dictionary<int, List<IBattleUnit>> UnitsDict { get; } = new();
        public BattleTeam Team { get; }

        public void InitGroups() { }
        public void RemoveUnitData(IBattleUnit unit) { }
    }
}