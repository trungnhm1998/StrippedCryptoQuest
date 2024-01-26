using CryptoQuest.Inventory.LootAPI;
using IndiGames.Core.Events;

namespace CryptoQuest.Inventory.Actions
{
    public class UpdateCharacterExpAction : ActionBase
    {
        public UpdateCharacterExpRequest.UpdateEXPBody[] UpdateEXPRequests;

        public UpdateCharacterExpAction(UpdateCharacterExpRequest.UpdateEXPBody[] updateEXPRequests)
        {
            UpdateEXPRequests = updateEXPRequests;
        }
    }
}