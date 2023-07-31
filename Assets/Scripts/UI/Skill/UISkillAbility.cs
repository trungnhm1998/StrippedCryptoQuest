using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using PolyAndCode.UI;
using CryptoQuest.Gameplay.Skill;

namespace CryptoQuest.UI.Skill
{
    public class UISkillAbility : MonoBehaviour, ICell
    {
        public Image Icon;
        public LocalizeStringEvent Name;
        public Text ManaPoint;
        public LocalizedString Description;
        private SkillInformation _skillInfo;

        public event UnityAction<SkillSO> Clicked;

        public void Init(SkillInformation skillInfo)
        {
            _skillInfo = skillInfo;
            Icon.sprite = skillInfo.SkillSO.Icon;
            Name.StringReference = skillInfo.SkillSO.Name;
            ManaPoint.text = skillInfo.SkillSO.Mana.ToString();
            Description = skillInfo.SkillSO.Description;
        }

        public void OnClicked()
        {
            Clicked?.Invoke(_skillInfo.SkillSO);
        }

        public void Select()
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}
