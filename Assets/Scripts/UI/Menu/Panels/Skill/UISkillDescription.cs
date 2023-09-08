using CryptoQuest.Gameplay.Skill;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    public class UISkillDescription : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _description;

        private void Awake()
        {
            UISkillButton.SelectingSkillEvent += Configure;
        }

        private void OnDestroy()
        {
            UISkillButton.SelectingSkillEvent -= Configure;
        }

        private void Configure(AbilityData abilityData)
        {
            _description.StringReference = abilityData.SkillInfo.SkillDescription;
        }
    }
}
