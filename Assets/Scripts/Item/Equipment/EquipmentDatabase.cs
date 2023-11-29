using IndiGames.Core.Database;

namespace CryptoQuest.Item.Equipment
{
    public class EquipmentDatabase : AssetReferenceDatabaseT<string, EquipmentDefSO>
    {
#if UNITY_EDITOR
        protected override string Editor_GetInstanceId(EquipmentDefSO asset) => asset.Data.ID;
#endif
    }
}