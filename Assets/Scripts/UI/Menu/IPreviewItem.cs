using CryptoQuest.Item;
using CryptoQuest.Item.Consumable;
using CryptoQuest.Item.Equipment;

namespace CryptoQuest.UI.Menu
{
    public interface IPreviewItem
    {
        void Preview(Equipment equipment);
        void Preview(ConsumableInfo consumable);
        void Hide();
    }
}