using UnityEngine;
using System.Collections;

namespace CryptoQuest.Gameplay.Battle
{
    public class MonsterUnit : CharacterUnit
    {
        private ISkillSelector _skillSelector;

        protected override void Start()
        {
            base.Start();
            _skillSelector = new RandomSkillSelector();
        }

        public override IEnumerator Prepare()
        {
            _selectedSkill = _skillSelector.GetSkill(this);
            SetDefaultTarget();
            yield return base.Prepare();
        }
    }
}