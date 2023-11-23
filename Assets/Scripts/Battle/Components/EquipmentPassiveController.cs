using System.Collections.Generic;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Item.Equipment;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class EquipmentPassiveController : MonoBehaviour
    {
        private EquipmentsController _equipmentsController;

        private readonly Dictionary<EquipmentInfo, PassiveAbilitySpec[]> _passives = new();
        private AbilitySystemBehaviour _abilitySystem;

        protected void Awake()
        {
            _equipmentsController = GetComponent<EquipmentsController>();
            _abilitySystem = GetComponent<AbilitySystemBehaviour>();

            _equipmentsController.Equipped += GrantPassive;
            _equipmentsController.Removed += RemovePassive;
        }

        private void OnDestroy()
        {
            _equipmentsController.Equipped -= GrantPassive;
            _equipmentsController.Removed -= RemovePassive;
        }

        private void GrantPassive(EquipmentInfo equipment)
        {
            var passiveSpecs = new PassiveAbilitySpec[equipment.Passives.Length];
            for (var index = 0; index < equipment.Passives.Length; index++)
            {
                var passive = equipment.Passives[index];
                if (passive == null) continue;
                var spec = _abilitySystem.GiveAbility<PassiveAbilitySpec>(passive);
                passiveSpecs[index] = spec;
            }

            if (passiveSpecs.Length > 0) _passives.Add(equipment, passiveSpecs);
        }

        private void RemovePassive(EquipmentInfo equipment)
        {
            if (_passives.TryGetValue(equipment, out var passives) == false) return;
            foreach (var passive in passives) _abilitySystem.RemoveAbility(passive);
            _passives.Remove(equipment);
        }
    }
}