using System.Collections;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility.ScriptableObjects;
using UnityEngine;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills
{
    /// <summary>
    /// Skill will end right after activate like normal attack
    /// </summary>
    [CreateAssetMenu(fileName = "CQSkillSO", menuName = "Gameplay/Battle/Abilities/Crypto Quest Skill")]
    public class CQSkillSO : EffectAbilitySO
    {
        public BattleActionDataSO ActionDataSO;
        public BattleActionDataEventChannelSO ActionEventSO;
        public LocalizedString SkillName;
        protected override GameplayAbilitySpec CreateAbility() => new CQSkill();
    }

    public class CQSkill : EffectGameplayAbilitySpec
    {
        protected const string UNIT_NAME_VARIABLE = "unitName";
        protected const string SKILL_NAME_VARIABLE = "skillName";
        protected IBattleUnit _unit;
        protected new CQSkillSO AbilitySO => (CQSkillSO)_abilitySO;

        public virtual void OnAbilityGranted(Skills.Ability skill)
        {
            base.OnAbilityGranted(skill);
            _unit = Owner.GetComponent<IBattleUnit>();
        }

        protected override IEnumerator InternalActiveAbility()
        {
            if (_unit == null) yield break;
            SkillActivatePromt();
            yield return base.InternalActiveAbility();
        }

        protected virtual void SkillActivatePromt()
        {
            var actionData = AbilitySO.ActionDataSO;
            if (actionData == null) return;

            actionData.Init(_unit.UnitLogic.TargetContainer[0]);
            actionData.AddStringVar(UNIT_NAME_VARIABLE, _unit.UnitInfo.DisplayName);
            AbilitySO.ActionEventSO.RaiseEvent(actionData);
        }
    }
}