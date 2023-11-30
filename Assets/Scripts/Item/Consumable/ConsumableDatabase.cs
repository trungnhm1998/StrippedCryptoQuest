using IndiGames.Core.Database;

namespace CryptoQuest.Item.Consumable
{
    public class ConsumableDatabase : AssetReferenceDatabaseT<string, ConsumableSO>
    {
#if UNITY_EDITOR
        protected override string Editor_GetInstanceId(ConsumableSO consumable) => consumable.ID;
#endif
    }
}
