using System.Collections.Generic;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Item.MagicStone;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Sagas.Equipment
{
    public abstract class StonePassiveRequest<T> : SagaBase<T> where T : StonePassiveRequestBase
    {
        [SerializeField] protected MagicStoneInventory _stoneInventory;
        [SerializeField] protected PartyManager _partyManager;
        protected override void HandleAction(T ctx) { }

        protected MagicStonePassiveController GetCharacterSpec(StonePassiveRequestBase ctx)
        {
            if (ctx.PassiveController != null)
                return ctx.PassiveController;
            foreach (var slot in _partyManager.Slots)
            {
                if (!slot.IsValid()) continue;
                var heroBehaviour = slot.HeroBehaviour;
                if (heroBehaviour.Spec.Id != ctx.CharacterID) continue;
                return heroBehaviour.GetComponent<MagicStonePassiveController>();
            }

            return null;
        }
    }

    public class ApplyStonePassiveOnCharacter : StonePassiveRequest<ApplyStonePassiveRequest>
    {
        protected override void HandleAction(ApplyStonePassiveRequest ctx)
        {
            var characterPassiveController = GetCharacterSpec(ctx);
            if (characterPassiveController == null) return;
            List<int> stoneIDs = new List<int>(ctx.StoneIDs);
            foreach (var stone in _stoneInventory.MagicStones)
            {
                if (!stone.IsValid()) continue;
                if (stone.AttachEquipmentId != ctx.EquipmentID || !stoneIDs.Contains(stone.ID)) continue;
                characterPassiveController.ApplyPassives(stone);
                stoneIDs.Remove(stone.ID);
            }
        }
    }
}