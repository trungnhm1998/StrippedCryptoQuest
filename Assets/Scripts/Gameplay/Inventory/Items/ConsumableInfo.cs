using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Inventory.Items
{
    [Serializable]
    public class ConsumableInfo : ItemInfo<ConsumableSO>
    {
        [field: SerializeField] public int Quantity { get; private set; } = 1;

        public Sprite Icon => Data.Image;
        public LocalizedString DisplayName => Data.DisplayName;
        public LocalizedString Description => Data.Description;

        public ConsumableInfo(ConsumableSO baseItemSO, int quantity = 1) : base(baseItemSO)
        {
            Quantity = quantity;
        }

        public ConsumableInfo() { }

        public void SetQuantity(int quantity)
        {
            Quantity = quantity;
        }

        private void Activate()
        {
            // TODO: REFACTOR GAS
            // CryptoQuestGameplayEffectSpec ability =
            //     (CryptoQuestGameplayEffectSpec)Owner.MakeOutgoingSpec(Data.Skill.Effect);
            //
            // ability.SetParameters(Data.ItemAbilityInfo.SkillParameters);
            // Owner.ApplyEffectSpecToSelf(ability);
        }

        public void ConsumeWithCorrectUI() => Data.Ability.Consuming();

        public ConsumableInfo Clone()
        {
            return new ConsumableInfo(Data, Quantity);
        }
    }
}