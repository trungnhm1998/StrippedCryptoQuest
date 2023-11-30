using CryptoQuest.System.TransitionSystem.States;
using UnityEngine;

namespace CryptoQuest.System.TransitionSystem.TransitionTypes
{
    [CreateAssetMenu(menuName = "Transition/Fade Out")]
    public class FadeOutSO : AbstractTransition
    {
        public override ITransitionState GetTransitionState(TransitionSystem transitionSystem)
        {
            return new FadeOutState(transitionSystem);
        }
    }
}