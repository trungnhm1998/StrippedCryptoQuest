using CryptoQuest.AbilitySystem.Abilities;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace CryptoQuest.UI.Menu.Panels.Skill
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
            _description.StringReference = skill.Parameters.SkillDescription;
        }
    }
}
