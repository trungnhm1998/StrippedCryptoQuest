using CryptoQuest.Battle.Events;

namespace CryptoQuest.Battle.Components
{
    public class HeroAttackBehaviour : NormalAttack
    {
        protected override void OnPreAttack(Character target) =>
            BattleEventBus.RaiseEvent(new HeroNormalAttackEvent { Target = target });
    }
}