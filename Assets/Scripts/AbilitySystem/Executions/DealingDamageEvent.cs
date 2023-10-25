using CryptoQuest.AbilitySystem.Attributes;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;

namespace CryptoQuest.AbilitySystem.Executions
{
    public class DealingDamageEvent : EffectExecutionCalculationBase
    {
        public delegate void ReceivedPhysicalDamageEvent(Battle.Components.Character dealer,
            Battle.Components.Character receiver, float damage);

        public event ReceivedPhysicalDamageEvent DamageDealt;

        public override void Execute(ref CustomExecutionParameters executionParams,
            ref GameplayEffectCustomExecutionOutput outModifiers)
        {
            foreach (var mod in outModifiers.Modifiers)
            {
                if (mod.Attribute != AttributeSets.Health) continue;
                if (executionParams.TargetAbilitySystemComponent.TryGetComponent(
                        out Battle.Components.Character character) == false) return;
                DamageDealt?.Invoke(
                    executionParams.SourceAbilitySystemComponent.GetComponent<Battle.Components.Character>(), character,
                    mod.Magnitude);
            }
        }
    }
}