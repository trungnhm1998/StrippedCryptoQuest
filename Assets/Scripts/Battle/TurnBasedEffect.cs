using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.Implementation.BasicEffect;
using UnityEngine;

namespace CryptoQuest.Battle
{
    [CreateAssetMenu(menuName = "Create TurnBasedEffect", fileName = "TurnBasedEffect", order = 0)]
    public class TurnBasedEffect : InfiniteEffectScriptableObject
    {
        /// <summary>
        /// How many turns this effect will last
        /// </summary>
        [field: SerializeField] public int TurnDuration { get; private set; }

        protected override GameplayEffectSpec CreateEffect() => new TurnBasedEffectSpec();
    }

    public class TurnBasedEffectSpec : GameplayEffectSpec { }
}