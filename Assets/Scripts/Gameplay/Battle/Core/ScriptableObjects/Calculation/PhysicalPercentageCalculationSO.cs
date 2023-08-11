using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Calculation
{
    public class PhysicalPercentageCalculationSO : AbstractEffectExecutionCalculationSO
    {
        [SerializeField] private AttributeScriptableObject _ownerAttack;
        [SerializeField] private AttributeScriptableObject _targetDefense;
        [SerializeField] private AttributeScriptableObject _targetedAttribute;
        [SerializeField] private float _lowerRandomRange = -0.05f;
        [SerializeField] private float _upperRandomRange = 0.05f;
        [SerializeField] private BattleActionDataSO _takeDamageActionData;
        [SerializeField] private BattleActionDataSO _deathActionData;
        [SerializeField] private BattleActionDataEventChannelSO _actionEventSO;
        private IBattleUnit _ownerUnit;
        private IBattleUnit _targetUnit;

        public override bool ExecuteImplementation(ref AbstractEffect effectSpec,
            ref EffectAttributeModifier[] modifiers)
        {
            _ownerUnit = effectSpec.Owner.GetComponent<IBattleUnit>();
            _targetUnit = effectSpec.Target.GetComponent<IBattleUnit>();
            SkillParameters skillParameters = effectSpec.Parameters as SkillParameters;
            if (skillParameters == null) return false;

            effectSpec.Owner.AttributeSystem.GetAttributeValue(_ownerAttack, out AttributeValue attackValue);
            effectSpec.Target.AttributeSystem.GetAttributeValue(_targetDefense, out AttributeValue defenseValue);
            float baseDamageValue = BattleCalculator.CalculateBaseDamage(skillParameters, attackValue.CurrentValue,
                Random.Range(_lowerRandomRange, _upperRandomRange));
            float damageValue = 0;
            if (skillParameters.IsFixed)
                damageValue = BattleCalculator.CalculateFixedPhysicalDamage(baseDamageValue, 1, 1);
            else
                damageValue =
                    BattleCalculator.CalculatePercentPhysicalDamage(baseDamageValue, attackValue.CurrentValue,
                        defenseValue.CurrentValue, 1, 1);

            for (var index = 0; index < modifiers.Length; index++)
            {
                if (modifiers[index].AttributeSO != _targetedAttribute) continue;
                modifiers[index].Value = damageValue * -1;
                LogAfterCalculateDamage(Mathf.Round(damageValue));
                LogIfTargetDeath(effectSpec, damageValue);
                return true;
            }

            return false;
        }

        private void LogAfterCalculateDamage(float damage)
        {
            if (_ownerUnit == null || _targetUnit == null) return;

            _takeDamageActionData.Init(_targetUnit.UnitInfo.Owner);
            _takeDamageActionData.AddStringVar("unitName", _targetUnit.UnitInfo.DisplayName);
            _takeDamageActionData.AddFloatVar("damage", damage);
            _actionEventSO.RaiseEvent(_takeDamageActionData);
        }

        private void LogIfTargetDeath(AbstractEffect effectSpec, float damage)
        {
            AbilitySystemBehaviour target = effectSpec.Target;
            target.AttributeSystem.GetAttributeValue(_targetedAttribute, out AttributeValue value);
            if (value.CurrentValue <= damage)
            {
                _deathActionData.Log.Clear();
                _deathActionData.AddStringVar("unitName", _targetUnit.UnitInfo.DisplayName);
                _actionEventSO.RaiseEvent(_deathActionData);
            }
        }
    }
}