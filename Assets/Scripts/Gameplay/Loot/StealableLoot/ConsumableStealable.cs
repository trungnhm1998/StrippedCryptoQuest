using System;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public class ConsumableStealable : StealableInfo<ConsumableLootInfo>
    {
        public override LocalizedString DisplayName => Loot.Item.DisplayName;
        public ConsumableStealable() : base(null) { }

        public ConsumableStealable(ConsumableLootInfo loot, float chanceToSteal = 1)
            : base(loot, chanceToSteal) { }
    }
}