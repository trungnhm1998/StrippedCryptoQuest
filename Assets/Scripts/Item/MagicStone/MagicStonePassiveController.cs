using System.Collections.Generic;
using System.Linq;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Components;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Sagas.Equipment;
using IndiGames.Core.Events;

namespace CryptoQuest.Item.MagicStone
{
    public class MagicStonePassiveController : CharacterComponentBase
    {
        private PassivesController _passiveController;
        private EquipmentsController _equipmentsController;
        private Dictionary<EquipmentStoneKeyID, PassiveAbilitySpec[]> _passiveAbilitySpecs = new();
        private HeroBehaviour _heroBehaviour;

        private void Awake()
        {
            _passiveController = GetComponent<PassivesController>();
            _equipmentsController = GetComponent<EquipmentsController>();
            _heroBehaviour = GetComponent<HeroBehaviour>();
            _equipmentsController.Equipped += GetEquipmentStoneAndApplyPassives;
            _equipmentsController.Removed += GetEquipmentStoneAndRemovePassives;
        }

        private void OnDestroy()
        {
            _equipmentsController.Equipped -= GetEquipmentStoneAndApplyPassives;
            _equipmentsController.Removed -= GetEquipmentStoneAndRemovePassives;
        }

        private void GetEquipmentStoneAndApplyPassives(IEquipment equipment)
        {
            var stones = equipment.Data.AttachStones;
            if (stones.Count <= 0 || stones.Any(x => x == 0)) return;
            ActionDispatcher.Dispatch(new ApplyStonePassiveRequest()
            {
                CharacterID = _heroBehaviour.Spec.Id,
                EquipmentID = equipment.Id,
                StoneIDs = stones,
                PassiveController = this
            });
        }

        private void GetEquipmentStoneAndRemovePassives(IEquipment equipment)
        {
            var stones = equipment.Data.AttachStones;
            if (stones.Count <= 0 || stones.Any(x => x == 0)) return;
            ActionDispatcher.Dispatch(new RemoveStonePassiveRequest()
            {
                CharacterID = _heroBehaviour.Spec.Id,
                EquipmentID = equipment.Id,
                StoneIDs = stones,
                PassiveController = this
            });
        }


        public void ApplyPassives(IMagicStone stone)
        {
            var key = new EquipmentStoneKeyID()
            {
                EquipmentID = stone.AttachEquipmentId,
                StoneID = stone.ID
            };
            var passiveSpecs = new PassiveAbilitySpec[stone.Passives.Length];
            for (var i = 0; i < stone.Passives.Length; i++)
            {
                var passive = stone.Passives[i];
                var spec = _passiveController.ApplyPassive(passive);
                passiveSpecs[i] = spec;
            }

            if (passiveSpecs.Length > 0) _passiveAbilitySpecs.Add(key, passiveSpecs);
        }

        public void RemovePassives(IMagicStone stone, int equipmentId)
        {
            var key = new EquipmentStoneKeyID();
            foreach (var cachedSpec in _passiveAbilitySpecs)
            {
                if (cachedSpec.Key.EquipmentID != equipmentId || cachedSpec.Key.StoneID != stone.ID) continue;
                key = cachedSpec.Key;
                break;
            }

            if (!_passiveAbilitySpecs.TryGetValue(key, out var specs)) return;
            foreach (var spec in specs)
                _passiveController.RemovePassive(spec);

            _passiveAbilitySpecs.Remove(key);
        }


        public void RemovePassives(IMagicStone stone)
        {
            foreach (var passive in stone.Passives) Character.AbilitySystem.RemoveAbility(passive);
        }

        private class EquipmentStoneKeyID
        {
            public int EquipmentID { get; set; }
            public int StoneID { get; set; }
        }
    }
}