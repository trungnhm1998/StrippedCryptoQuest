using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Battle.Helper;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit
{
    public class HeroUnitLogic : BaseBattleUnitLogic
    {
        public HeroUnitLogic(IBattleUnit unit, BattleUnitTagConfigSO tagConfig)
            : base(unit, tagConfig) { }

        public override void PerformUnitAction()
        {
            _owner.ActivateAbilityWithTag(_tagConfig.BeforeActionTag);
            base.PerformUnitAction();
            _owner.ActivateAbilityWithTag(_tagConfig.AfterActionTag);
        }
    }
}