using UnityEngine;


namespace Indigames.AbilitySystem
{
    // [CreateAssetMenu(fileName = "DefaultEffectExecutionCalculation", menuName = "Indigames Ability System/Effects/Execution Calculations/Default")]
    public class DefaultEffectExecutionCalculationSO : AbstractEffectExecutionCalculationSO
    {
        public override bool ExecuteImplementation(ref AbstractEffect effectSpec, ref EffectAttributeModifier[] attributeModifiers)
        {
            return true;
        }
    }
}