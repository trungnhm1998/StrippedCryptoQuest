using CryptoQuest.Core;
using UnityEngine;

namespace CryptoQuest.Gameplay.Loot
{
    public class LootDatabase : GenericAssetReferenceDatabase<int, LootTable> 
    {
#if UNITY_EDITOR
        [ContextMenu("Fetch Data In Project")]
        public override void Editor_FetchDataInProject()
        {
            base.Editor_FetchDataInProject();
        }

        protected override void Editor_SetInstanceId(ref Map instance, LootTable table)
        {
            if (!int.TryParse(table.ID, out var id))
            {
                instance.Id = 0;
                return;
            }
            instance.Id = id;
        }
#endif
    }
}