using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    [CreateAssetMenu(menuName = "Create BattleBus Item Test", fileName = "BattleBus", order = 0)]
    public class MockBattleItemBus : BattleBus
    {
        public ItemName[] Mobs = new[]
        {
            new ItemName()
            {
                name = "Sword",
            },
            new ItemName()
            {
                name = "lightSaber",
            },
            new ItemName()
            {
                name = "Wood",
            }
        };
    }
}