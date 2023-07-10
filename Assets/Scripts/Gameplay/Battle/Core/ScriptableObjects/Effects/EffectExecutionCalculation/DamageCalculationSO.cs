using UnityEngine;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using UnityEngine.Localization.Settings;
using System.Collections.Generic;

namespace CryptoQuest.Gameplay.Battle
{
    [CreateAssetMenu(fileName = "DamageCaculation", menuName = "Gameplay/Battle/Effects/Execution Calculations/Damage Caculation")]
    public class DamageCalculationSO : AbstractEffectExecutionCalculationSO
    {
        public string TakeDamagePromtKey;
        public string MissPromtKey;
        public string NoDamagePromtKey;
        public string DeathPromtKey;
        public AttributeScriptableObject OwnerAttack;
        public AttributeScriptableObject TargetHP;

        private IBattleUnit _ownerUnit;
        private IBattleUnit _targetUnit;
        private const string BATTLE_PROMT_TABLE = "BattlePromt";
        private Dictionary<string, string> _cachedStrings = new();

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

                    if (_ownerUnit == null || _targetUnit == null) return true;
                    string _takeDamagePromt = GetCachedString(TakeDamagePromtKey);
                    _ownerUnit.ExecuteLogs.Add(string.Format(_takeDamagePromt, damageValue, effectSpec.Target.gameObject.name));

                    CheckTargetDeath(effectSpec, damageValue);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// TODO: Check when localize changed to clear cache
        /// </summary>
        /// <param name="promtKey"></param>
        /// <returns></returns>
        private string GetCachedString(string promtKey)
        {
            if (promtKey == "") return null;
            if (_cachedStrings.TryGetValue(promtKey, out string value))
            {
                return value;
            }
            string localizedString = LocalizationSettings.StringDatabase.GetLocalizedString(BATTLE_PROMT_TABLE, promtKey);
            _cachedStrings.Add(promtKey, localizedString);
            return localizedString;
        }

        private void CheckTargetDeath(AbstractEffect effectSpec, float damage)
        {
            AbilitySystemBehaviour target = effectSpec.Target;
            target.AttributeSystem.GetAttributeValue(TargetHP, out AttributeValue value);
            if (value.CurrentValue <= damage)
            {
                string _deathPromt = GetCachedString(DeathPromtKey);
                _ownerUnit.ExecuteLogs.Add(string.Format(_deathPromt, target.gameObject.name));
            }
        }
    }
}