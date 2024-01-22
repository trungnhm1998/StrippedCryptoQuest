using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.PlayerParty;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Inventory
{
    public class AddExpToPartyAction : ActionBase
    {
        public float Exp;

        public AddExpToPartyAction(float exp)
        {
            Exp = exp;
        }
    }

    public class PartyExpController : SagaBase<AddExpToPartyAction>
    {
        [SerializeField] private PartyManager _partyManager;

        private void AddExpToParty(float exp)
        {
            foreach (var slot in _partyManager.Slots)
            {
                if (!slot.HeroBehaviour.IsValidAndAlive()) continue;
                var levelSystem = slot.HeroBehaviour.GetComponent<LevelSystem>();
                levelSystem.AddExp(exp);
            }
        }

        protected override void HandleAction(AddExpToPartyAction ctx)
        {
            AddExpToParty(ctx.Exp);
        }
    }
}