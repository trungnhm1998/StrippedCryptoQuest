using System;
using CryptoQuest.Battle.Components.SpecialSkillBehaviours;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public class ConsumableStealable : StealableInfo<UsableLootInfo>
    {
        public override LocalizedString DisplayName => Loot.Item.DisplayName;

        public ConsumableStealable(UsableLootInfo loot, float chanceToSteal = 1)
            : base(loot, chanceToSteal) { }
    }
}