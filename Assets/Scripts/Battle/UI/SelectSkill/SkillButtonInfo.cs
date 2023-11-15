using System;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.UI.CommandDetail;
using CryptoQuest.UI.Common;
using UnityEngine;

namespace CryptoQuest.Battle.UI.SelectSkill
{
    [Serializable]
    public class SkillButtonInfo : ButtonInfoBase
    {
        public CastSkillAbility Skill { get; private set; }
        private Action<CastSkillAbility> _skillCallback;
        private UIGrayoutTextsBehaviour _grayoutBehaviour;

        public SkillButtonInfo(CastSkillAbility skill, Action<CastSkillAbility> skillCallback)
            : base("", $"{skill.MpToCast.ToString()}")
        {
            Skill = skill;
            LocalizedLabel = skill.SkillName;
            if (LocalizedLabel.IsEmpty)
            {
                Label = skill.name;
            }
            _skillCallback = skillCallback;
        }

        public void SetSelectable(bool selectable)
        {
            if (_grayoutBehaviour == null)
            {
                _grayoutBehaviour = Button.GetComponent<UIGrayoutTextsBehaviour>();
            }

            _grayoutBehaviour.SetGrayoutTexts(selectable);

            if (!selectable)
                _skillCallback = null;
        }

        public override void OnHandleClick()
        {
            _skillCallback?.Invoke(Skill);
        }
    }
}