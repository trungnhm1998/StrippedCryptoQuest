using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using IndiGames.Core.Database;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects
{
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