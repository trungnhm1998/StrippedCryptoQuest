using CryptoQuest.Beast;
using IndiGames.Core.Events;

namespace CryptoQuest.Menus.Beast.Sagas
{
    public class UpdateBeastEquippedAction : ActionBase
    {
        public int BeastId { get; }

        public UpdateBeastEquippedAction(IBeast beast)
        {
            BeastId = beast.Id;
        }
    }
}