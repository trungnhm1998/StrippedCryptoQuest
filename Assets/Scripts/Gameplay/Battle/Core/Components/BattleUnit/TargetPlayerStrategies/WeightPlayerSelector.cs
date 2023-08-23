using CryptoQuest.Gameplay.Battle.Helper;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using System.Linq;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit.TargetPlayerStrategies
{
    public class WeightPlayerSelector : IPlayerHeroSelector
    {
        public AbilitySystemBehaviour GetTarget(MonsterUnit monsterUnit)
        {
            var weightConfig = monsterUnit.TargetWeightConfig;
            var playerUnits = monsterUnit.OpponentTeam.BattleUnits;
            var weightRandomIndex = CommonBattleHelper.WeightedRandomIndex(weightConfig.Weights[0..playerUnits.Count]);
            var isIndexValid = (0 <= weightRandomIndex) && (weightRandomIndex < playerUnits.Count());
            var selectedUnit = isIndexValid ? playerUnits[weightRandomIndex] : playerUnits[0];
            return selectedUnit.Owner;
        }
    }
}