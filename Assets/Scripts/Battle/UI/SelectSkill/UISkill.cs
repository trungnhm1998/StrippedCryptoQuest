using System;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.UI.Common;
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
        [SerializeField] private UIGrayoutTextsBehaviour _grayoutBehaviour;

        private CastSkillAbility _skill;
        public CastSkillAbility Skill => _skill;


        public void Init(CastSkillAbility skill, bool isCastable = true)
        {
            _skill = skill;
            _name.text = skill.name;
            _nameStringEvent.StringReference = skill.SkillName;
            _cost.text = skill.MpToCast.ToString();
            SetSelectable(isCastable);
        }

        public void SetSelectable(bool selectable)
        {
            _grayoutBehaviour.SetGrayoutTexts(selectable);
            if (!selectable) Selected = null;
        }

        public void OnPressed()
        {
            Selected?.Invoke(this);
        }
    }
}