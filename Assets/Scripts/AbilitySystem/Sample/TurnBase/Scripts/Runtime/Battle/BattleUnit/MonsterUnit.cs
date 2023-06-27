using UnityEngine;
using System.Collections;

namespace Indigames.AbilitySystem.Sample
{
    public class MonsterUnit : BattleUnitBase
    {
        private ISkillActivator _skillActivator;

        private void Start()
        {
            _skillActivator = new RandomSkillActivator();
        }

        public override IEnumerator Execute()
        {
            yield return base.Execute();
            _skillActivator.ActivateSkill(this);
            // Select character then random perform skill
            yield return null;
        }
    }
}