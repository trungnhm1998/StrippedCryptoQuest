using IndiGames.Core.Database;

namespace CryptoQuest.Item.Equipment
{
    public class EquipmentDatabaseSO : GenericAssetReferenceDatabase<string, EquipmentPrefab>
    {
#if UNITY_EDITOR
        protected override string Editor_GetInstanceId(EquipmentPrefab equipment) => equipment.ID;
#endif
    }
}