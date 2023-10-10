using System;
using CryptoQuest.Character.Ability;
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
            _nameStringEvent.StringReference = skill.Parameters.SkillName;
            _cost.text = skill.Parameters.Cost.ToString();
        }

        public void OnPressed()
        {
            Selected?.Invoke(this);
        }
    }
}