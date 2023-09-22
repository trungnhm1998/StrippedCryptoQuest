using IndiGames.Core.Database;

namespace CryptoQuest.Gameplay.Loot
{
    public class LootDatabase : GenericAssetReferenceDatabase<int, LootTable>
    {
#if UNITY_EDITOR
        protected override int Editor_GetInstanceId(LootTable table) => table.ID;
#endif
    }
}