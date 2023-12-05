namespace CryptoQuest.Item.MagicStone
{
    public interface IStoneInventoryController
    {
        bool Add(IMagicStone stone);
        bool Remove(IMagicStone stone);
    }
}