using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    [CreateAssetMenu(menuName = "Create BattleBus Test", fileName = "BattleBus", order = 0)]
    public class MockBattleAttackBus : BattleBus
    {
        public Mob[] Mobs = new[]
        {
            new Mob()
            {
                name = "Slime",
            },
            new Mob()
            {
                name = "Goblin",
            },
            new Mob()
            {
                name = "Orc",
            }
        };
    }
}