using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Skill;
using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    public class UISkill : MonoBehaviour, ICell
    {
        [SerializeField] private Image _skillIcon;
        [SerializeField] private LocalizeStringEvent _skillName;
        [SerializeField] private Text _quantity;

        public Gameplay.Skill.Skill CachedSkill { get; private set; }

        public void Configure(Gameplay.Skill.Skill skill)
        {
            CachedSkill = skill;
            _skillName.StringReference = skill.SkillInfo.SkillName;
        }
    }
}
