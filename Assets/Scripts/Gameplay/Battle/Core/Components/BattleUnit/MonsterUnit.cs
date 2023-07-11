using UnityEngine;
using System.Collections;

namespace CryptoQuest.Gameplay.Battle
{
    public class MonsterUnit : CharacterUnit
    {
        private ISkillSelector _skillSelector;

        protected void Awake()
        {
            _skillSelector = new RandomSkillSelector();
        }

        public override IEnumerator Prepare()
        {
            SelectedSkill = _skillSelector.GetSkill(this);
            SetDefaultTarget();
            yield return base.Prepare();
        }
    }
}