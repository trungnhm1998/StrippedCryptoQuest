using UnityEngine;
using System.Collections;

namespace Indigames.AbilitySystem.Sample
{
    public class MonsterUnit : BattleUnitBase
    {
        private ISkillSelector _skillSelector;

        private void Start()
        {
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