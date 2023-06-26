using UnityEngine;

namespace Indigames.AbilitySystem
{
    public abstract class ModifierComputationSO : ScriptableObject
    {
        /// <summary>
        /// Function called when the magnitude is calculated, usually after the target has been assigned
        /// In my case it will be in IEffectApplier
        /// </summary>
        /// <param name="effect">Effect Specification</param>
        /// <returns></returns>
        public abstract float CalculateMagnitude(AbstractEffect effect);

        /// <summary>
        /// Show preview value if you apply this effect
        /// </summary>
        /// <param name="effect">Effect Specification</param>
        /// <returns></returns>
        public virtual float CalculatePreview(AbstractEffect effect){
            return 0;
        }
    }
}