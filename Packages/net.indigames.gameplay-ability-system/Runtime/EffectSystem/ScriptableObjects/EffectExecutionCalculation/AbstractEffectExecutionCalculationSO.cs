using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation
{
    public abstract class AbstractEffectExecutionCalculationSO : ScriptableObject
    {
        /// <summary>
        /// Custom logic for calculating the effect modifier before it is applied to the target.
        /// such as calculate the damage based on the target's defense and owner's attack damage.
        /// by default this will do nothing
        /// </summary>
        /// <param name="effectSpec"></param>
        /// <param name="modifiers">Modifier which will be alter</param>
        public abstract bool ExecuteImplementation(ref AbstractEffect effectSpec, ref EffectAttributeModifier[] modifiers);
    }
}