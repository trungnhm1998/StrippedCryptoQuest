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

        private readonly Dictionary<IEquipment, PassiveAbilitySpec[]> _passives = new();
        private PassivesController _passiveController;

        protected void Awake()
        {
            _equipmentsController = GetComponent<EquipmentsController>();
            _passiveController = GetComponent<PassivesController>();

            _equipmentsController.Equipped += GrantPassive;
            _equipmentsController.Removed += RemovePassive;
        }

        private void OnDestroy()
        {
            _equipmentsController.Equipped -= GrantPassive;
            _equipmentsController.Removed -= RemovePassive;
        }

        private void GrantPassive(IEquipment equipment)
        {
            var passiveSpecs = new PassiveAbilitySpec[equipment.Passives.Length];
            for (var index = 0; index < equipment.Passives.Length; index++)
            {
                var passive = equipment.Passives[index];
                if (passive == null) continue;
                var spec = _passiveController.ApplyPassive(passive);
                passiveSpecs[index] = spec;
            }

            if (passiveSpecs.Length > 0) _passives.Add(equipment, passiveSpecs);
        }

        private void RemovePassive(IEquipment equipment)
        {
            if (_passives.TryGetValue(equipment, out var passives) == false) return;
            foreach (var passive in passives) _passiveController.RemovePassive(passive);
            _passives.Remove(equipment);
        }
    }
}