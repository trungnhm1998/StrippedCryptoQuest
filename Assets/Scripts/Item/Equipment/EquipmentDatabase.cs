using IndiGames.Core.Database;

namespace CryptoQuest.Item.Equipment
{
    public class EquipmentDatabase : AssetReferenceDatabaseT<string, EquipmentSO>
    {
#if UNITY_EDITOR
        protected override string Editor_GetInstanceId(EquipmentSO asset) => asset.Data.ID;
#endif
    }
}