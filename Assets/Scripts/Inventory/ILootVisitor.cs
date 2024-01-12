using CryptoQuest.Gameplay.Loot;

namespace CryptoQuest.Inventory
{
    public interface ILootVisitor
    {
        void Visit(ConsumableLootInfo loot);
        void Visit(CurrencyLootInfo loot);
        void Visit(EquipmentLoot loot);
        void Visit(ExpLoot loot);
        void Visit(MagicStoneLoot loot);
    }
}