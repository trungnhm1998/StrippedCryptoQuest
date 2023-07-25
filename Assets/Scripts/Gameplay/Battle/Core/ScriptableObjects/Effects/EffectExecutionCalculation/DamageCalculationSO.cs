using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;
using UnityEngine.Localization.Settings;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Effects.EffectExecutionCalculation
{
    [CreateAssetMenu(fileName = "DamageCaculation", menuName = "Gameplay/Battle/Effects/Execution Calculations/Damage Caculation")]
    public class DamageCalculationSO : AbstractEffectExecutionCalculationSO
    {
        public BattleActionDataEventChannelSO ActionEventSO;
        public BattleActionDataSO TakeDamageActionData;
        public BattleActionDataSO MissActionData;
        public BattleActionDataSO NoDamageActionData;
        public BattleActionDataSO DeathActionData;
        public AttributeScriptableObject OwnerAttack;
        public AttributeScriptableObject TargetHP;

        private IBattleUnit _ownerUnit;
        private IBattleUnit _targetUnit;

        public override bool ExecuteImplementation(ref AbstractEffect effectSpec,
            ref EffectAttributeModifier[] modifiers)
        {
            effectSpec.Owner.AttributeSystem.GetAttributeValue(OwnerAttack, out var attackDamage);
            _ownerUnit = effectSpec.Owner.GetComponent<IBattleUnit>();
            _targetUnit = effectSpec.Target.GetComponent<IBattleUnit>();

            modifiers = effectSpec.EffectSO.EffectDetails.Modifiers;
            float damageValue = attackDamage.CurrentValue;

            for (var index = 0; index < modifiers.Length; index++)
            {
                EffectAttributeModifier effectAttributeModifier = modifiers[index];
                if (effectAttributeModifier.AttributeSO == TargetHP)
                {
                    EffectAttributeModifier previousModifier = effectAttributeModifier;
                    previousModifier.Value = damageValue * -1;
                    modifiers[index] = previousModifier;

                    LogAfterCalculateDamage(damageValue);
                    LogIfTargetDeath(effectSpec, damageValue);
                    return true;
                }
            }

            return false;
        }

        private void LogAfterCalculateDamage(float damage)
        {
            if (_ownerUnit == null || _targetUnit == null) return;
            CharacterDataSO targetUnitData = _targetUnit.UnitData;
            if (targetUnitData == null) return;

            TakeDamageActionData.Log.Clear();
            TakeDamageActionData.AddStringVar("unitName", targetUnitData.DisplayName);
            TakeDamageActionData.AddFloatVar("damage", damage);
            ActionEventSO.RaiseEvent(TakeDamageActionData);
        }

        private void LogIfTargetDeath(AbstractEffect effectSpec, float damage)
        {
            AbilitySystemBehaviour target = effectSpec.Target;
            target.AttributeSystem.GetAttributeValue(TargetHP, out AttributeValue value);
            if (value.CurrentValue <= damage)
            {
                CharacterDataSO targetUnitData = _targetUnit.UnitData;
                if (targetUnitData == null) return;

                DeathActionData.Log.Clear();
                DeathActionData.AddStringVar("unitName", targetUnitData.DisplayName);
                ActionEventSO.RaiseEvent(DeathActionData);
            }
        }
    }
}