using CryptoQuest.States;
using CryptoQuest.System.TransitionSystem;
using CryptoQuest.System.TransitionSystem.States;
using UnityEngine;

namespace CryptoQuest.TransitionTypes
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