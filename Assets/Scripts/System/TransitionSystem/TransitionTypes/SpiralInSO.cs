using CryptoQuest.System.TransitionSystem.States;
using UnityEngine;

namespace CryptoQuest.System.TransitionSystem.TransitionTypes
{
    [CreateAssetMenu(menuName = "Transition/Spiral In")]
    public class SpiralInSO : AbstractTransition
    {
        public override ITransitionState GetTransitionState(TransitionSystem transitionSystem)
        {
            return new SpiralInState(transitionSystem);
        }
    }
}