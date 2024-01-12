using IndiGames.Core.Events;

namespace CryptoQuest.Item.MagicStone.Sagas
{
    public class AddMagicStoneAction : ActionBase
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
    }

    public class AddRewardedMagicStoneAction : ActionBase
    {
        public string Id { get; set; }
        public int Quantity { get; set; }

        public AddRewardedMagicStoneAction(string id, int quantity)
        {
            Id = id;
            Quantity = quantity;
        }
    }
}