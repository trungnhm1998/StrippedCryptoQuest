using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    [CreateAssetMenu(menuName = "Create BattleBus Skill Test", fileName = "BattleBus", order = 0)]
    public class MockBattleSkillBus : BattleBus
    {
        public SkillName[] Skill = new[]
        {
            new SkillName()
            {
                name = "Fire",
            },
            new SkillName()
            {
                name = "Water",
            },
            new SkillName()
            {
                name = "Lightning",
            }
        };
    }
}