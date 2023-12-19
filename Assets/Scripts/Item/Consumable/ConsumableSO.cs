using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.ScriptableObjects;
using CryptoQuest.Inventory.ScriptableObjects.Item.Type;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Item.Consumable
{
    [CreateAssetMenu(fileName = "Usable Item", menuName = "Crypto Quest/Inventory/Usable Item")]
    public class ConsumableSO : GenericItem
    {
        [field: SerializeField, Header("Usable Item")]
        public EConsumableType Type { get; private set; }

        [field: SerializeField] public int Price { get; private set; }
        [field: SerializeField] public int SellPrice { get; private set; }

        [field: SerializeField] public GameplayEffectDefinition Effect { get; private set; }
        [field: SerializeField] public ConsumeItemAbility Ability { get; private set; }
        [field: SerializeField] public EAbilityUsageScenario UsageScenario { get; private set; }

        /// <summary>
        /// Derived class should raise event to show correct UI if there is any
        ///
        /// currently we have behavior:
        /// - Single target hero
        /// - Ocarina with special UI flow
        /// - Target all hero in party (This doesn't have UI flow yet)
        /// </summary>
        [field: SerializeField] public VoidEventChannelSO TargetSelectionEvent { get; private set; }

#if UNITY_EDITOR
        public void Editor_SetUsableType(EConsumableType type)
        {
            Type = type;
        }

        public void Editor_SetEffect(GameplayEffectDefinition effect)
        {
            Effect = effect;
        }

        public void Editor_SetAbility(ConsumeItemAbility ability)
        {
            Ability = ability;
        }
#endif
    }
}