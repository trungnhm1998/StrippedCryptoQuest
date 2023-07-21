using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    [CreateAssetMenu(menuName = "Create BattleBus Mob Test", fileName = "BattleBus", order = 0)]
    public class MockBattleMobBus : BattleBus
    {
        public Mob[] Mobs = new[]
        {
            new Mob()
            {
                name = "iHeysd",
            },
            new Mob()
            {
                name = "Yooo",
            },
            new Mob()
            {
                name = "Vip",
            }
        };
    }
}