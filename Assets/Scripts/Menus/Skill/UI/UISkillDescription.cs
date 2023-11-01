using CryptoQuest.AbilitySystem.Abilities;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace CryptoQuest.Menus.Skill.UI
{
    public class UISkillDescription : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _description;

        private void Awake()
        {
            UISkill.InspectingSkillEvent += Configure;
        }

        private void OnDestroy()
        {
            UISkill.InspectingSkillEvent -= Configure;
        }

        private void Configure(CastSkillAbility skill)
        {
            _description.StringReference = skill.SkillInfo.SkillDescription;
        }
    }
}
