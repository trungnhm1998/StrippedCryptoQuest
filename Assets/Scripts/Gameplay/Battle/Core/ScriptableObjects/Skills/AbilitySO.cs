using System.Collections;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills.CryptoQuestAbility
{
    [CreateAssetMenu(fileName = "CQ Ability", menuName = "Gameplay/Battle/Abilities/CQ Ability")]
    public class AbilitySO : EffectAbilitySO
    {
        public SkillInfo SkillInfo;
        public AttributeScriptableObject MPAttributeSO;
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

            Owner.AttributeSystem.GetAttributeValue(AbilitySO.MPAttributeSO, out var mpAttributeValue);
            if (!IsMpEnoughToCast(mpAttributeValue)) return;
            SetRemainingMp(mpAttributeValue.CurrentValue - _skillInfo.MPConsumption);

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

        private bool IsMpEnoughToCast(AttributeValue mpAttributeValue)
        {
            bool isEnough = mpAttributeValue.CurrentValue >= _skillInfo.MPConsumption;
            if (!isEnough)
                Debug.Log("Not enough MP to cast");
            return mpAttributeValue.CurrentValue >= _skillInfo.MPConsumption;
        }

        private void SetRemainingMp(float value)
        {
            Owner.AttributeSystem.SetAttributeBaseValue(AbilitySO.MPAttributeSO, value);
        }
    }
}