using CryptoQuest.States;
using CryptoQuest.System.TransitionSystem;
using UnityEngine;

namespace CryptoQuest.TransitionTypes
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