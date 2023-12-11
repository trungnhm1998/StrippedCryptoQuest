using System.Collections.Generic;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Components;
using CryptoQuest.Beast.Interface;
using CryptoQuest.Beast.ScriptableObjects;
using CryptoQuest.Gameplay.PlayerParty;

namespace CryptoQuest.Beast
{
    public class BeastPassiveApplier : IBeastPassiveApplier
    {
        private IBeastEquippingBehaviour _behaviour;
        private Dictionary<PartySlot, PassiveAbilitySpec> _beastPassiveMap;

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
            _beastPassiveMap = new Dictionary<PartySlot, PassiveAbilitySpec>();
        }
        
        public void ApplyPassive(IBeast beast)
        {
            if (_equippedBeast.IsValid()) RemovePassive(_equippedBeast);
            if (!beast.IsValid()) return;
            foreach (var partySlot in _partyController.Slots)
            {
                if (partySlot.IsValid() == false) continue;
                var passiveController = partySlot.HeroBehaviour.GetComponent<PassivesController>();
                var passiveSpec = passiveController.ApplyPassive(beast.Passive);
                _beastPassiveMap[partySlot] = passiveSpec;
            }
            _equippedBeast = beast;
        }


        private void RemovePassive(IBeast beast)
        {
            foreach (var partySlot in _partyController.Slots)
            {
                if (_beastPassiveMap.TryGetValue(partySlot, out var passive) == false) return;
                if (partySlot.IsValid() == false) continue;
                var passiveController = partySlot.HeroBehaviour.GetComponent<PassivesController>();
                passiveController.RemovePassive(passive);
                _beastPassiveMap.Remove(partySlot);
            }
            _equippedBeast = NullBeast.Instance;
        }
    }
}