using System.Linq;
using CryptoQuest.Beast.Interface;
using CryptoQuest.Gameplay.PlayerParty;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace CryptoQuest.Beast
{
    public class BeastPassiveController : IBeastPassiveController
    {
        private IBeastEquippingBehaviour _behaviour;
        private IBeast _equippedBeast
        {
            get => _behaviour.EquippingBeast;
            set => _behaviour.EquippingBeast = value;
        }
        private IPartyController _partyController;
        public BeastPassiveController(IBeastEquippingBehaviour behaviour, IPartyController partyController)
        {
            _behaviour = behaviour;
            _partyController = partyController;
        }

        public void ApplyPassive(IBeast beast)
        {
            if (beast == null) return;
            var beastPassive = beast.Passive;
            if (beast.Passive == null) return;
            if (_equippedBeast != null) RemovePassive(_equippedBeast);
            foreach (var partySlot in _partyController.Slots)
            {
                if (partySlot.IsValid() == false) continue;
                var hero = partySlot.HeroBehaviour;
                hero.AbilitySystem.GiveAbility(beastPassive);
            }
            _equippedBeast = beast;
        }

        public void RemovePassive(IBeast beast)
        {
            if (beast == null) return;
            var beastPassive = beast.Passive;
            if (beastPassive == null) return;
            foreach (var partySlot in _partyController.Slots)
            {
                if (partySlot.IsValid() == false) continue;
                var abilitySystem = partySlot.HeroBehaviour.GetComponent<AbilitySystemBehaviour>();
                var abilityToRemove = abilitySystem.GrantedAbilities.FirstOrDefault(ability => ability.AbilitySO == beastPassive);

                if (abilityToRemove != null)
                {
                    abilitySystem.RemoveAbility(abilityToRemove);
                }
            }
        }
    }
}