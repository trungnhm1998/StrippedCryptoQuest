using CryptoQuest.Item.MagicStone;
using IndiGames.Core.Events;

namespace CryptoQuest.Inventory.Actions
{
    public class StoneActionBase : ActionBase
    {
        public IMagicStone Stone { get; }

        protected StoneActionBase(IMagicStone stone)
        {
            Stone = stone;
        }
    }

    public class AddStoneAction : StoneActionBase
    {
        public AddStoneAction(IMagicStone stone) : base(stone) { }
    }

    public class RemoveStoneAction : StoneActionBase
    {
        public RemoveStoneAction(IMagicStone stone) : base(stone) { }
    }
}