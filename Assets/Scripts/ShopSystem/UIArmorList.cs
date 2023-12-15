using CryptoQuest.Item.Equipment;

namespace CryptoQuest.ShopSystem
{
    public class UIArmorList : UIEquipmentList
    {
        protected override bool IsIgnoreType(IEquipment item) =>
            item.Type.EquipmentCategory == EEquipmentCategory.Weapon;
    }
}