using CryptoQuest.Character.Ability;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item
{
    [CreateAssetMenu(fileName = "Usable Item", menuName = "Crypto Quest/Inventory/Usable Item")]
    public class ConsumableSO : GenericItem
    {
        // TODO: Use enum https://github.com/indigames/CryptoQuestClient/issues/1406
        [field: SerializeField, Header("Usable Item")]
        public ConsumableType consumableType { get; private set; }

        [field: SerializeField] public ConsumeItemAbility Ability { get; private set; }
        [field: SerializeField] public EAbilityUsageScenario UsageScenario { get; private set; }

#if UNITY_EDITOR
        public void Editor_SetUsableType(ConsumableType type)
        {
            consumableType = type;
        }
#endif
    }
}