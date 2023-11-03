using UnityEngine;
using UnityEngine.Localization.Components;

namespace CryptoQuest.Menus.Skill.UI
{
    public class UISkillDescription : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _description;

        private void Awake() => UISkill.InspectingSkillEvent += Configure;

        private void OnDestroy() => UISkill.InspectingSkillEvent -= Configure;

        private void Configure(UISkill skillUI)
        {
            _description.StringReference = skill.SkillDescription;
            _description.RefreshString();
        }
    }
}