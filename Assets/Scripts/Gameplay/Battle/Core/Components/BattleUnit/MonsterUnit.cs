using System.Collections;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit.AbilitySelectStrategies;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit
{
    public class MonsterUnit : CharacterUnit
    {
        private IAbilitySelector _abilitySelector;

        protected void Awake()
        {
            _abilitySelector = new RandomAbilitySelector();
        }

        public override IEnumerator Prepare()
        {
            SelectAbility(_abilitySelector.GetAbility(this));
            SetDefaultTarget();
            yield return base.Prepare();
        }
    }
}