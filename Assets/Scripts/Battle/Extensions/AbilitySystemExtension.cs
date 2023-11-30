using CryptoQuest.AbilitySystem;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using CharacterBehaviour = CryptoQuest.Battle.Components.Character;

namespace CryptoQuest.Battle.Extensions
{
    public static class AbilitySystemExtension
    {
        public static bool CheckSealedMagicSkill(this CastSkillAbilitySpec skillSpec)
        {
            if (skillSpec.IsSealedMagicSkill())
            {
                var character = skillSpec.Owner.GetComponent<CharacterBehaviour>();
                BattleEventBus.RaiseEvent(new SealedEvent()
                {
                    Character = character,
                    Tag = TagsDef.SealMagic
                });
                return true;
            }
            return false;
        }

        public static bool CheckSealedPhysicSkill(this CastSkillAbilitySpec skillSpec)
        {
            if (skillSpec.IsSealedPhysicSkill())
            {
                var character = skillSpec.Owner.GetComponent<CharacterBehaviour>();
                BattleEventBus.RaiseEvent(new SealedEvent()
                {
                    Character = character,
                    Tag = TagsDef.SealPhysic
                });
                return true;
            }
            return false;
        }

        public static bool IsCastable(this CastSkillAbility skill, AbilitySystemBehaviour owner)
        {
            var spec = owner.GiveAbility<CastSkillAbilitySpec>(skill);
            if (!spec.CheckCost() || spec.IsSealedMagicSkill() || spec.IsSealedPhysicSkill())
                return false;

            return true;
        }

        public static bool IsSealedMagicSkill(this CastSkillAbilitySpec skillSpec)
        {
            return skillSpec.Def.CheckPreventCastSkillWithTag(skillSpec.Owner,
                TagsDef.SealMagic, ESkillType.Magic);
        }

        public static bool IsSealedPhysicSkill(this CastSkillAbilitySpec skillSpec)
        {
            return skillSpec.Def.CheckPreventCastSkillWithTag(skillSpec.Owner,
                TagsDef.SealPhysic, ESkillType.Physical);
        }

        private static bool CheckPreventCastSkillWithTag(this CastSkillAbility skill, AbilitySystemBehaviour owner, 
            TagScriptableObject preventTag, ESkillType typeToCheck)
        {
            var skillContext = skill.Context;
            if (skillContext.SkillInfo.SkillType != typeToCheck) return false;
            return owner.TagSystem.HasTag(preventTag);
        }
    }
}