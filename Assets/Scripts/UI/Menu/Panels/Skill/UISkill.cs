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

        public AbilityData CachedAbilityData { get; private set; }

        public void Configure(AbilityData abilityData)
        {
            CachedAbilityData = abilityData;
            _skillName.StringReference = abilityData.SkillInfo.SkillName;
        }
    }
}
