using IndiGames.Core.Events;

namespace CryptoQuest.Inventory.Actions
{
    public class UpdateCharacterExpAction : ActionBase
    {
        public int CharacterId;
        public float UpdatedExp;

        public UpdateCharacterExpAction(int characterId, float updatedExp)
        {
            CharacterId = characterId;
            UpdatedExp = updatedExp;
        }
    }
}