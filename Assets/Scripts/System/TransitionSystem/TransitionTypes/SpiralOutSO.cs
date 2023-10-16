using CryptoQuest.States;
using CryptoQuest.System.TransitionSystem;
using UnityEngine;

namespace CryptoQuest.TransitionTypes
{
    [CreateAssetMenu(menuName = "Transition/Spiral Out")]
    public class SpiralOutSO : AbstractTransition
    {
        public override ITransitionState GetTransitionState(TransitionSystem transitionSystem)
        {
            return new SpiralOutState(transitionSystem);
        }
    }
}