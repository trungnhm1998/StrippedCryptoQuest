using System.Collections;
using CryptoQuest.Battle.Components;

namespace CryptoQuest.AbilitySystem.Abilities
{
    public abstract class PostNormalAttackPassiveSpecBase : PassiveAbilitySpec
    {
        protected override IEnumerator OnAbilityActive()
        {
            if (Character.TryGetComponent(out NormalAttack normalAttack))
                normalAttack.Attacked += OnAttacked;
            yield break;
        }

        protected override void OnAbilityEnded()
        {
            if (Character.TryGetComponent(out NormalAttack normalAttack))
                normalAttack.Attacked -= OnAttacked;
        }

        protected abstract void OnAttacked();
    }
}