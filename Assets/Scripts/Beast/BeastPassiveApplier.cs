using System.Collections.Generic;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Beast.Interface;
using CryptoQuest.Beast.ScriptableObjects;
using CryptoQuest.Gameplay.PlayerParty;

namespace CryptoQuest.Beast
{
    public class BeastPassiveApplier : IBeastPassiveApplier
    {
        private IBeastEquippingBehaviour _behaviour;
        private Dictionary<IBeast, PassiveAbility> _beastPassiveMap;

        private IBeast _equippedBeast
        {
            get => _behaviour.EquippingBeast;
            set => _behaviour.EquippingBeast = value;
        }

        private IPartyController _partyController;

        public BeastPassiveApplier(IBeastEquippingBehaviour behaviour, IPartyController partyController)
        {
            _behaviour = behaviour;
            _partyController = partyController;
            _beastPassiveMap = new Dictionary<IBeast, PassiveAbility>();
        }

        public void ApplyPassive(IBeast beast)
        {
            beast ??= NullBeast.Instance;
            var passive = beast.Passive == null ? NullBeast.Instance.Passive : beast.Passive;
            if (_equippedBeast != null) RemovePassive(_equippedBeast);
            foreach (var partySlot in _partyController.Slots)
            {
                if (partySlot.IsValid() == false) continue;
                var hero = partySlot.HeroBehaviour;
                _beastPassiveMap[beast] = passive;
                hero.AbilitySystem.GiveAbility(passive);
            }
            _equippedBeast = beast;
        }

        private void RemovePassive(IBeast beast)
        {
            beast ??= NullBeast.Instance;
            var passive = _beastPassiveMap.ContainsKey(beast) ? _beastPassiveMap[beast] : NullBeast.Instance.Passive;
            foreach (var partySlot in _partyController.Slots)
            {
                if (partySlot.IsValid() == false) continue;
                var abilitySystem = partySlot.HeroBehaviour.AbilitySystem;
                abilitySystem.RemoveAbility(passive);
            }

            _beastPassiveMap.Remove(beast);
        }
    }
}