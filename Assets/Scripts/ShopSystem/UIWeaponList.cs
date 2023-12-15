using CryptoQuest.Item.Equipment;

namespace CryptoQuest.ShopSystem
{
    public class UIWeaponList : UIEquipmentList
    {
        protected override bool IsIgnoreType(IEquipment item)
        {
            return item.Type.EquipmentCategory != EEquipmentCategory.Weapon;
        }
    }
}