namespace CryptoQuest.Item.MagicStone
{
    public interface IStoneInventoryController
    {
        bool Add(MagicStoneInfo stoneInfo);
        bool Remove(MagicStoneInfo stoneInfo);
    }
}