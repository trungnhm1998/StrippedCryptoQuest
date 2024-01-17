using System.Collections.Generic;
using CryptoQuest.Item.MagicStone;

namespace CryptoQuest.Sagas.Equipment
{
    public class RemoveStonePassiveOnCharacter : StonePassiveRequest<RemoveStonePassiveRequest>
    {
        protected override void HandleAction(RemoveStonePassiveRequest ctx)
        {
            var characterPassiveController = GetCharacterSpec(ctx);
            List<int> stoneIDs = new List<int>(ctx.StoneIDs);
            foreach (var stone in _stoneInventory.MagicStones)
            {
                if (!stone.IsValid()) continue;
                if (!stoneIDs.Contains(stone.ID)) continue;
                characterPassiveController.RemovePassives(stone, ctx.EquipmentID);
                stoneIDs.Remove(stone.ID);
            }
        }
    }
}