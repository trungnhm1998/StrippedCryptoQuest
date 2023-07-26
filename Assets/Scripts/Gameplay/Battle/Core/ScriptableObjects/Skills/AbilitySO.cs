using System.Collections;
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
        public override AbilityParameters Parameters => SkillInfo.SkillParameters;
        protected override AbstractAbility CreateAbility() => new Ability(SkillInfo);
    }

    public class Ability : EffectAbility
    {
        private SkillInfo _skillInfo;
        protected new AbilitySO AbilitySO => (AbilitySO)_abilitySO;

        public Ability(SkillInfo skillInfo)
        {
            _skillInfo = skillInfo;
        }

        public override void ActivateAbility()
        {
            if (!CanActiveAbility()) return;

            Owner.AttributeSystem.GetAttributeValue(AbilitySO.CostSpecSO, out var ownerCostSpec);
            if (!IsValidCost(ownerCostSpec)) return;
            SetRemainingCostSpec(ownerCostSpec.CurrentValue - _skillInfo.Cost);

            _isActive = true;
            Owner.StartCoroutine(InternalActiveAbility());
            Owner.TagSystem.AddTags(AbilitySO.Tags.ActivationTags);
        }


        public override void OnAbilityGranted(AbstractAbility skillSpec)
        {
            base.OnAbilityGranted(skillSpec);
        }

        protected override IEnumerator InternalActiveAbility()
        {
            yield return base.InternalActiveAbility();
        }

        private bool IsValidCost(AttributeValue costSpec)
        {
            bool isEnough = costSpec.CurrentValue >= _skillInfo.Cost;
            if (!isEnough)
                Debug.Log("Not enough to cast");
            return isEnough;
        }

        private void SetRemainingCostSpec(float value)
        {
            Owner.AttributeSystem.SetAttributeBaseValue(AbilitySO.CostSpecSO, value);
        }
    }
}