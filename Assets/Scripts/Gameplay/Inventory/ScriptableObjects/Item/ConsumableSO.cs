using CryptoQuest.Character.Ability;
using CryptoQuest.Gameplay.Battle.Core;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.ActionTypes;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item
{
    [CreateAssetMenu(fileName = "Usable Item", menuName = "Crypto Quest/Inventory/Usable Item")]
    public class ConsumableSO : GenericItem
    {
        [field: Header("Usable Item")]
        [field: SerializeField] public ConsumableType consumableType { get; private set; }

        [field: SerializeField] public ActionDefinitionBase ActionDefinition { get; private set; }
        [field: SerializeField] public ConsumeItemAbility Ability { get; private set; }

        [field: SerializeField] public CastableAbility Skill { get; private set; }

        public ActionSpecificationBase Action => ActionDefinition.Create();

        [field: SerializeField] public EAbilityUsageScenario UsageScenario { get; private set; }

        [field: Header("Item Effect Info"), SerializeField]
        public SkillInfo ItemAbilityInfo { get; private set; }

#if UNITY_EDITOR
        /// <summary>
        /// Make sure ability of this item has item's name
        /// to show in battle when select item
        /// </summary>
        private void OnValidate()
        {
            if (Skill == null) return;
        }

        public void Editor_SetUsableType(ConsumableType type)
        {
            consumableType = type;
        }
#endif
    }
}