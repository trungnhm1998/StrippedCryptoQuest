using UnityEngine;

namespace Indigames.AbilitySystem
{
    [CreateAssetMenu(fileName = "ConstantFloatComputation", menuName = "Indigames Ability System/Effects/Modifier Calculation/Default Constant Float Calculation")]
    public class ConstantFloatComputationSO : ModifierComputationSO
    {
        public override float CalculateMagnitude(AbstractEffect effect)
        {
            return 0;
        }
    }
}