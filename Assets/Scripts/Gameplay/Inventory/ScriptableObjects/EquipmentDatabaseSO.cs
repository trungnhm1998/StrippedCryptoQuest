using CryptoQuest.Core;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using UnityEngine;

namespace CryptoQuest.Gameplay.Loot
{
    [CreateAssetMenu(fileName = "EquipmentDatabase", menuName = "Create EquipmentDatabase")]
    public class EquipmentDatabaseSO : GenericAssetReferenceDatabase<string, EquipmentSO>
    {
#if UNITY_EDITOR
        [ContextMenu("Fetch Data In Project")]
        public override void Editor_FetchDataInProject()
        {
            base.Editor_FetchDataInProject();
        }

        protected override void Editor_SetInstanceId(ref Map instance, EquipmentSO equipment)
        {
            instance.Id = equipment.ID;
        }
#endif
    }
}