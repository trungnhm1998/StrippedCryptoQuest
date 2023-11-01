using System;
using CryptoQuest.AbilitySystem.Abilities;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace CryptoQuest.Battle.UI.SelectSkill
{
    public class UISkill : MonoBehaviour
    {
        public Action<UISkill> Selected;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _cost;
        [SerializeField] private LocalizeStringEvent _nameStringEvent;

        private CastSkillAbility _skill;
        public CastSkillAbility Skill => _skill;

        public void Init(CastSkillAbility skill)
        {
            _skill = skill;
            _name.text = skill.name;
            _nameStringEvent.StringReference = skill.SkillInfo.SkillName;
            _cost.text = skill.MpToCast.ToString();
        }

        public void OnPressed()
        {
            Selected?.Invoke(this);
        }
    }
}