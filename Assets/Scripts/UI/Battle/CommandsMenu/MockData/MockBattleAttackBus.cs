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
                name = "chingchong",
            },
            new Mob()
            {
                name = "somethingElse2",
            },
            new Mob()
            {
                name = "nigga",
            }
        };
    }
}