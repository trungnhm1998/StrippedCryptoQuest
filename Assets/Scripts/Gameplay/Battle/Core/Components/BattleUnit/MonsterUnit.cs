using System.Collections;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit.SkillSelectStrategies;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit
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