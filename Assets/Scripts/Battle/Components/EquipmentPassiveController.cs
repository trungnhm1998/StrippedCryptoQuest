using System.Collections.Generic;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Item.Equipment;

namespace CryptoQuest.Battle.Components
{
    public class EquipmentPassiveController : CharacterComponentBase
    {
        private EquipmentsController _equipmentsController;

        private readonly Dictionary<EquipmentInfo, PassiveAbilitySpec[]> _passives = new();

        protected override void Awake()
        {
            base.Awake();
            _equipmentsController = GetComponent<EquipmentsController>();

            // _equipmentsController.Equipped += GrantPassive;
            // _equipmentsController.Removed += RemovePassive;
        }

        private void OnDestroy()
        {
            // _equipmentsController.Equipped -= GrantPassive;
            // _equipmentsController.Removed -= RemovePassive;
        }

        public override void Init() { }

        private void GrantPassive(EquipmentInfo equipment)
        {
            if(equipment.Passives.Length > 0)
            {
                var passiveSpecs = new PassiveAbilitySpec[equipment.Passives.Length];
                for (var index = 0; index < equipment.Passives.Length; index++)
                {
                    var passive = equipment.Passives[index];
                    if (passive == null) continue;
                    var spec = Character.AbilitySystem.GiveAbility<PassiveAbilitySpec>(passive);
                    passiveSpecs[index] = spec;
                }
                _passives.Add(equipment, passiveSpecs);
            }
        }

        private void RemovePassive(EquipmentInfo equipment)
        {
            if (_passives.TryGetValue(equipment, out var passives) == false) return;
            foreach (var passive in passives)
            {
                Character.AbilitySystem.RemoveAbility(passive);
            }

            _passives.Remove(equipment);
        }
    }
}