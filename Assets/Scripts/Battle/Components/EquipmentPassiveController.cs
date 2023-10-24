using System;
using System.Collections.Generic;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Item.Equipment;

namespace CryptoQuest.Battle.Components
{
    public class EquipmentPassiveController : CharacterComponentBase
    {
        private EquipmentsController _equipmentsController;

        private readonly Dictionary<EquipmentInfo, PassiveAbilitySpec> _passives =
            new Dictionary<EquipmentInfo, PassiveAbilitySpec>();

        protected override void Awake()
        {
            base.Awake();
            _equipmentsController = GetComponent<EquipmentsController>();

            _equipmentsController.Equipped += GrantPassive;
            _equipmentsController.Removed += RemovePassive;
        }

        private void OnDestroy()
        {
            _equipmentsController.Equipped -= GrantPassive;
            _equipmentsController.Removed -= RemovePassive;
        }

        public override void Init() { }

        private void GrantPassive(EquipmentInfo equipment)
        {
            PassiveAbility passive = equipment.Passive;
            if (passive == null) return;
            var spec = Character.AbilitySystem.GiveAbility<PassiveAbilitySpec>(passive);
            _passives.Add(equipment, spec);
        }

        private void RemovePassive(EquipmentInfo equipment)
        {
            if (_passives.TryGetValue(equipment, out var passive) == false) return;
            Character.AbilitySystem.RemoveAbility(passive);
            _passives.Remove(equipment);
        }
    }
}