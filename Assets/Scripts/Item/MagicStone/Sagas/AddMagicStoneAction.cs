using IndiGames.Core.Events;

namespace CryptoQuest.Item.MagicStone.Sagas
{
    public class AddMagicStoneAction : ActionBase
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
    }
}