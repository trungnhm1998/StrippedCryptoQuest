using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;

namespace CryptoQuest.UI.Menu
{
    public interface IPreviewItem
    {
        void Preview(EquipmentInfo equipment);
        void Preview(ConsumableInfo consumable);
        void Hide();
    }
}