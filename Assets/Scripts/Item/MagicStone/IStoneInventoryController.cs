namespace CryptoQuest.Item.MagicStone
{
    public interface IStoneInventoryController
    {
        bool Add(MagicStone stone);
        bool Remove(MagicStone stone);
    }
}