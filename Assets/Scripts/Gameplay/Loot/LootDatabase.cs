using IndiGames.Core.Database;

namespace CryptoQuest.Gameplay.Loot
{
    public class LootDatabase : AssetReferenceDatabaseT<int, LootTable>
    {
#if UNITY_EDITOR
        protected override int Editor_GetInstanceId(LootTable table) => table.ID;
#endif
    }
}