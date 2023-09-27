using IndiGames.Core.Database;

namespace CryptoQuest.Item.Equipment
{
    public class EquipmentDefineDatabase : AssetReferenceDatabaseT<string, EquipmentDef>
    {
#if UNITY_EDITOR
        protected override string Editor_GetInstanceId(EquipmentDef asset) => asset.ID;
#endif
    }
}