using System.Collections;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills
{
    [CreateAssetMenu(fileName = "CQ Ability", menuName = "Gameplay/Battle/Abilities/CQ Ability")]
    public class AbilitySO : EffectAbilitySO
    {
        public SkillInfo SkillInfo;
        public AttributeScriptableObject CostSpecSO;
        public BattleActionDataSO ActionDataSO;
        public BattleActionDataEventChannelSO ActionEventSO;
        protected override GameplayAbilitySpec CreateAbility() => new Ability(SkillInfo);
    }

    public class Ability : EffectGameplayAbilitySpec
    {
        private SkillInfo _skillInfo;
        protected const string UNIT_NAME_VARIABLE = "unitName";
        protected const string SKILL_NAME_VARIABLE = "skillName";
        public new AbilitySO AbilitySO => (AbilitySO)_abilitySO;
        protected IBattleUnit _unit;

        public Ability(SkillInfo skillInfo)
        {
            _skillInfo = skillInfo;
        }

        public override void ActivateAbility()
        {
            if (!CanActiveAbility()) return;
            _unit ??= Owner.GetComponent<IBattleUnit>();

            Owner.AttributeSystem.TryGetAttributeValue(AbilitySO.CostSpecSO, out var ownerCostSpec);
            if (!IsValidCost(ownerCostSpec)) return;
            SetRemainingCostSpec(ownerCostSpec.CurrentValue - _skillInfo.Cost);

            _isActive = true;
            Owner.StartCoroutine(InternalActiveAbility());
            Owner.TagSystem.AddTags(AbilitySO.Tags.ActivationTags);
        }

        protected override IEnumerator InternalActiveAbility()
        {
            OnSkillActivatePromt();
            yield return base.InternalActiveAbility();
        }

        private bool IsValidCost(AttributeValue costSpec)
        {
            bool isEnough = costSpec.CurrentValue >= _skillInfo.Cost;
            if (!isEnough)
                Debug.Log("Not enough to cast");
            return isEnough;
        }

        protected virtual void OnSkillActivatePromt()
        {
            var actionData = AbilitySO.ActionDataSO;
            if (actionData == null || _unit == null || _unit.UnitLogic == null) return;

            actionData.Init(_unit.UnitLogic.TargetContainer[0]);
            actionData.AddStringVar(UNIT_NAME_VARIABLE, _unit.UnitInfo.DisplayName);
            actionData.AddStringVar(SKILL_NAME_VARIABLE, AbilitySO.SkillInfo.SkillName.GetLocalizedString());
            AbilitySO.ActionEventSO.RaiseEvent(actionData);
        }

        private void SetRemainingCostSpec(float value)
        {
            Owner.AttributeSystem.SetAttributeBaseValue(AbilitySO.CostSpecSO, value);
        }
    }
}