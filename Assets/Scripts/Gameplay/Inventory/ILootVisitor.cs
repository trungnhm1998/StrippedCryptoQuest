using CryptoQuest.Gameplay.Loot;

namespace CryptoQuest.Gameplay.Inventory
{
    public interface ILootVisitor
    {
        void Visit(ConsumableLootInfo loot);
        void Visit(CurrencyLootInfo loot);
        void Visit(EquipmentLoot loot);
        void Visit(ExpLoot loot);
    }
}