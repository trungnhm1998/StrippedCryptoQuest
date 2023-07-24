using System.Collections;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills.CryptoQuestAbility
{
    [CreateAssetMenu(fileName = "CQ Ability", menuName = "Gameplay/Battle/Abilities/CQ Ability")]
    public class AbilitySO : EffectAbilitySO
    {
        public SkillInfo SkillInfo;
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

        public override void OnAbilityGranted(AbstractAbility skillSpec)
        {
            base.OnAbilityGranted(skillSpec);
        }

        protected override IEnumerator InternalActiveAbility()
        {
            yield return base.InternalActiveAbility();
        }
    }
}