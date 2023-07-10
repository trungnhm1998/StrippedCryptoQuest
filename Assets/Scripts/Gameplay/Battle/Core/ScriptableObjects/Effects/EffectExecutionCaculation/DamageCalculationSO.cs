using UnityEngine;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using UnityEngine.Localization.Settings;

namespace CryptoQuest.Gameplay.Battle
{
    [CreateAssetMenu(fileName = "DamageCaculation", menuName = "Gameplay/Battle/Effects/Execution Calculations/Damage Caculation")]
    public class DamageCalculationSO : AbstractEffectExecutionCalculationSO
    {
        public string DamagePromtKey;
        public string MissPromtKey;
        public string NoDamagePromtKey;
        public string DeathPromtKey;
        public AttributeScriptableObject OwnerAttack;
        public AttributeScriptableObject TargetHP;

        private IBattleUnit _ownerUnit;
        private IBattleUnit _targetUnit;
        private const string BATTLE_PROMT_TABLE = "BattlePromt";
        private string _damagePromt;
        private string _missPromt;
        private string _noDamagePromt;
        private string _deathPromt;

        public override bool ExecuteImplementation(ref AbstractEffect effectSpec,
            ref EffectAttributeModifier[] modifiers)
        {
            effectSpec.Owner.AttributeSystem.GetAttributeValue(OwnerAttack, out var attackDamage);
            InitUnits(effectSpec);
            InitTexts();

            modifiers = effectSpec.EffectSO.EffectDetails.Modifiers;
            float damageValue = attackDamage.CurrentValue;

            for (var index = 0; index < modifiers.Length; index++)
            {
                EffectAttributeModifier effectAttributeModifier = modifiers[index];
                if (effectAttributeModifier.AttributeSO != TargetHP) continue;

                EffectAttributeModifier previousModifier = effectAttributeModifier;
                previousModifier.Value = damageValue * -1;
                modifiers[index] = previousModifier;
                
                if (_ownerUnit == null) return true;
                _ownerUnit.ExecuteLogs.Add(string.Format(_damagePromt, damageValue, _targetUnit.OriginalName));
                return true;
            }

            return false;
        }

        private void InitUnits(AbstractEffect effectSpec)
        {
            if (_ownerUnit == null)
            {
                _ownerUnit = effectSpec.Owner.GetComponent<IBattleUnit>();
            }

            if (_targetUnit == null)
            {
                _targetUnit = effectSpec.Target.GetComponent<IBattleUnit>();
            }
        }

        private void InitTexts()
        {
            if (_damagePromt == "")
            {
                _damagePromt = LocalizationSettings.StringDatabase.GetLocalizedString(BATTLE_PROMT_TABLE, DamagePromtKey);
            }
            
            if (_missPromt == "")
            {
                _missPromt = LocalizationSettings.StringDatabase.GetLocalizedString(BATTLE_PROMT_TABLE, MissPromtKey);
            }

            if (_noDamagePromt == "")
            {
                _noDamagePromt = LocalizationSettings.StringDatabase.GetLocalizedString(BATTLE_PROMT_TABLE, NoDamagePromtKey);
            }

            if (_deathPromt == "")
            {
                _deathPromt = LocalizationSettings.StringDatabase.GetLocalizedString(BATTLE_PROMT_TABLE, DeathPromtKey);
            }
        }
    }
}