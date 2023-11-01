using CryptoQuest.AbilitySystem;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Events;
using CharacterBehaviour = CryptoQuest.Battle.Components.Character;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace CryptoQuest.Gameplay.Battle.Core.Helper
{
    public static class AbilitySystemExtension
    {
        public static bool CheckSealedMagicSkill(this CastSkillAbilitySpec skillSpec)
        {
            if (skillSpec.Def.CheckPreventCastSkillWithTag(skillSpec.Owner,
                TagsDef.SealMagic, ESkillType.Magic))
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
            if (skillSpec.Def.CheckPreventCastSkillWithTag(skillSpec.Owner,
                TagsDef.SealPhysic, ESkillType.Physical))
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

        private static bool CheckPreventCastSkillWithTag(this CastSkillAbility skill, AbilitySystemBehaviour owner, 
            TagScriptableObject preventTag, ESkillType typeToCheck)
        {
            var skillContext = skill.Context;
            if (skillContext.SkillInfo.SkillType != typeToCheck) return false;
            return owner.TagSystem.HasTag(preventTag);
        }
    }
}