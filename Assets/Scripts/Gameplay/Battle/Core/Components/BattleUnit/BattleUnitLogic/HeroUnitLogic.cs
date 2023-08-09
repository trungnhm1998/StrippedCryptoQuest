using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using System.Linq;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit
{
    public class HeroUnitLogic : BaseBattleUnitLogic
    {
        public HeroUnitLogic(IBattleUnit unit, BattleUnitTagConfigSO tagConfig) : base(unit, tagConfig)
        {
        }

        public override void PerformUnitAction()
        {
            ActivateAbilityWithTag(_tagConfig.BeforeActionTag);
            base.PerformUnitAction();
            ActivateAbilityWithTag(_tagConfig.AfterActionTag);
        }
    }
}