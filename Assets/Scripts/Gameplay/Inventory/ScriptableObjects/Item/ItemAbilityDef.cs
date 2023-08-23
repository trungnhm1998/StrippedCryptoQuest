using CryptoQuest.Gameplay.Battle.Core;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item
{
    [CreateAssetMenu(menuName = "Crypto Quest/Ability/Item Ability", fileName = "Item Ability")]
    public class ItemAbilityDef : AbilitySO
    {
        protected override AbstractAbility CreateAbility() => new ItemAbility(SkillInfo);
    }

    public class ItemAbility : Ability
    {
        public ItemAbility(SkillInfo skillInfo) : base(skillInfo) { }

        public override void TryActiveAbility()
        {
            foreach (var containerMap in AbilitySO.EffectContainerMap)
            {
                foreach (var effectSpecs in containerMap.TargetContainer)
                {
                    foreach (var effect in effectSpecs.Effects)
                    {
                        var effectSpec = CreateEffectSpec(effect);
                        Owner.EffectSystem.ApplyEffectToSelf(effectSpec);
                    }
                }
            }
        }
    }
}