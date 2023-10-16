using CryptoQuest.States;
using CryptoQuest.System.TransitionSystem;
using UnityEngine;

namespace CryptoQuest.TransitionTypes
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