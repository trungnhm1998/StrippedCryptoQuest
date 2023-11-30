using CryptoQuest.System.TransitionSystem.States;
using UnityEngine;

namespace CryptoQuest.System.TransitionSystem.TransitionTypes
{
    [CreateAssetMenu(menuName = "Transition/Fade In")]
    public class FadeInSo : AbstractTransition
    {
        public override ITransitionState GetTransitionState(TransitionSystem transitionSystem)
        {
            return new FadeInState(transitionSystem);
        }
    }
}