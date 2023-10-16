using CryptoQuest.States;
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