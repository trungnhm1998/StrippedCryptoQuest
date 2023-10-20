using CryptoQuest.Battle.Events;

namespace CryptoQuest.Battle.Components
{
    class EnemyAttackBehaviour : NormalAttack
    {
        protected override void OnPreAttack(Character target) => BattleEventBus.RaiseEvent(new ShakeUIEvent());
    }
}