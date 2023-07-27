using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;

namespace CryptoQuest.UI.Battle.BattleActionHandler
{
    public class RetreatActionHandler : BattleActionHandler
    {
        public class NormalAttackHandler : BattleActionHandler
        {
            public AbilityScriptableObject RetreatAbilitySO;

            public override void Handle(IBattleUnit currentUnit)
            {
                if (currentUnit == null) return;
                AbilitySystemBehaviour currentUnitOwner = currentUnit.Owner;

                AbstractAbility retreatAbility = currentUnitOwner.GiveAbility(RetreatAbilitySO);
                currentUnit.SelectSkill(retreatAbility);
                base.Handle(currentUnit);
            }
        }
    }
}