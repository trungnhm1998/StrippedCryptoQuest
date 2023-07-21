using System;
using TMPro;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    class UISkillContent : ContentItemMenu
    {
        [SerializeField] private TextMeshProUGUI _label;

        public class Skill : BarDataStructure
        {
            public SkillName skillname;
        }

        public override BarDataStructure Foo()
        {
            return new Skill();
        }

        public override void Init(BarDataStructure input)
        {
           var skill = input as Skill;
           _label.text = skill.skillname.name;
        }
    }
}