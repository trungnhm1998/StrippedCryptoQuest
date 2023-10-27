using System.Collections;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Battle.Core;

namespace CryptoQuest.AbilitySystem.Abilities
{
    public struct PostAttackContext
    {
        public Battle.Components.Character Attacker;
        public Battle.Components.Character Target;
        public float Damage;
    }

    public class PostNormalAttackContext : GameplayEffectContext
    {
        public PostNormalAttackContext(GameplayEffectContext ctx) : base(ctx.SkillInfo) { }
        public PostAttackContext AttackContext { get; set; }
    }

    public abstract class PostNormalAttackPassiveSpecBase : PassiveAbilitySpec
    {
        protected override IEnumerator OnAbilityActive()
        {
            if (Character.TryGetComponent(out NormalAttack normalAttack))
                normalAttack.Attacked += NotifyPostAttack;
            yield break;
        }

        protected override void OnAbilityEnded()
        {
            if (Character.TryGetComponent(out NormalAttack normalAttack))
                normalAttack.Attacked -= NotifyPostAttack;
        }

        private void NotifyPostAttack(Battle.Components.Character attacker, Battle.Components.Character target,
            float damage) =>
            OnAttacked(new PostAttackContext()
            {
                Attacker = attacker,
                Target = target,
                Damage = damage
            });

        protected abstract void OnAttacked(PostAttackContext postAttackContext);
    }
}