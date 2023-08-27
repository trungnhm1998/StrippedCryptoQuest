using CryptoQuest.Gameplay.Skill;
using CryptoQuest.Menu;
using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    public class UISkillAbilityButton : MultiInputButton, ICell
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private Text _manaPointText;
        [field: SerializeField] public LocalizedString Description { get; private set; }
        [field: SerializeField] public ECharacterSkill TypeOfSkill { get; private set; }
        private SkillInformation _skillInfo;

        public void Init(SkillInformation skillInfo)
        {
            _skillInfo = skillInfo;
            _iconImage.sprite = skillInfo.abilitySo.SkillInfo.SkillIcon;
            _name.StringReference = skillInfo.abilitySo.SkillInfo.SkillName;
            _manaPointText.text = skillInfo.abilitySo.SkillInfo.Cost.ToString();
            Description = skillInfo.abilitySo.SkillInfo.SkillDescription;
            TypeOfSkill = ECharacterSkill.TargetCast;
        }
    }
}