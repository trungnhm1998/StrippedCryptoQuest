using System;
using CryptoQuest.Battle.Components.SpecialSkillBehaviours;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public abstract class StealableInfo
    {
        public abstract LocalizedString DisplayName { get; }
        public abstract bool IsValid();
        public abstract bool TryGiveLoot(IStealerBehaviour stealer);
        public abstract LootInfo GetLoot();
    }

    [Serializable]
    public abstract class StealableInfo<T> : StealableInfo where T : LootInfo
    {
        [SerializeField] private float _chanceToSteal;
        public float ChanceToSteal => _chanceToSteal;

        [SerializeField] private T _loot;
        public T Loot => _loot;
        public override LootInfo GetLoot() => _loot;

        public StealableInfo(T loot, float chanceToSteal = 1f)
        {
            _loot = loot;
            _chanceToSteal = chanceToSteal;
        }

        public override bool IsValid() => _loot != null && _loot.IsValid();
        
        public override bool TryGiveLoot(IStealerBehaviour stealer)
        {
            var randomChance = UnityEngine.Random.Range(0f, 1f);
            if (randomChance > ChanceToSteal) return false;
            stealer.Steal(this);

            return true;
        }
    }
}