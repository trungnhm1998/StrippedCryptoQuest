using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Indigames.AbilitySystem
{
    public class SkillSystem : MonoBehaviour
    {
        protected SkillSpecificationContainer _grantedSkills;
        public SkillSpecificationContainer GrantedSkills => _grantedSkills;

        protected AbilitySystem _owner;
        public AbilitySystem Owner => _owner;

        public void InitSystem(AbilitySystem owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// Add/Give/Grant skill to the system. Only skill that in the system can be active
        /// There's only 1 skill per system (no duplicate skill)
        /// </summary>
        /// <param name="skillDef"></param>
        /// <param name="abilityParams"></param>
        /// <returns>A skill handler (humble object) to execute their skill logic</returns>
        public AbstractSkill GiveSkill(SkillScriptableObject skillDef, AbilityParameters abilityParams)
        {
            foreach (var skill in _grantedSkills.Skills)
            {
                if (skill.SkillSO == skillDef)
                    return skill;
            }

            var grantedSkill = skillDef.GetSkillSpec(Owner, abilityParams);

            return GiveSkill(grantedSkill);
        }

        public AbstractSkill GiveSkill(AbstractSkill inSkillSpec)
        {
            if (!inSkillSpec.SkillSO) return null;

            foreach (AbstractSkill skillSpec in _grantedSkills.Skills)
            {
                if (skillSpec.SkillSO == inSkillSpec.SkillSO)
                    return skillSpec;
            }
            _grantedSkills.Skills.Add(inSkillSpec);
            OnGrantedSkill(inSkillSpec);

            return inSkillSpec;
        }

        private void OnGrantedSkill(AbstractSkill skillSpec)
        {
            if (!skillSpec.SkillSO) return;
            skillSpec.Owner = Owner;
            skillSpec.OnSkillGranted(skillSpec);
        }

        public void TryActiveSkill(AbstractSkill inSkillSpec)
        {
            if (inSkillSpec.SkillSO == null) return;
            foreach (var skillSpec in _grantedSkills.Skills)
            {
                if (skillSpec != inSkillSpec) continue;
                inSkillSpec.ActivateSkill();
            }
        }

        public bool RemoveSkill(AbstractSkill skill)
        {
            List<AbstractSkill> grantedSkillsList = _grantedSkills.Skills;
            for (int i = grantedSkillsList.Count - 1; i >= 0; i--)
            {
                var skillSpec = grantedSkillsList[i];
                if (skillSpec == skill)
                {
                    OnRemoveSkill(skillSpec);
                    grantedSkillsList.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }
        
        private void OnRemoveSkill(AbstractSkill skillSpec)
        {
            if (!skillSpec.SkillSO) return;

            skillSpec.OnSkillRemoved(skillSpec);
        }
        
        public void RemoveAllSkills()
        {
            for (int i = _grantedSkills.Skills.Count - 1; i >= 0; i--)
            {
                var skillSpec = _grantedSkills.Skills[i];
                _grantedSkills.Skills.RemoveAt(i);
                OnRemoveSkill(skillSpec);
            }
            _grantedSkills.Skills = new List<AbstractSkill>();
        }
        
        private void Update()
        {
            RemovePendingSkills();
        }

        protected virtual void RemovePendingSkills()
        {
            foreach (var skill in _grantedSkills.Skills.ToList())
            {
                if (skill.IsPendingRemove || skill.IsRemoveAfterActivation)
                    RemoveSkill(skill);
            }
        }
    }
}
