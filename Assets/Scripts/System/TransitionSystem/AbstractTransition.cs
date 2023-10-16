using UnityEngine;

namespace CryptoQuest
{
    public abstract class AbstractTransition : ScriptableObject
    {
        public abstract ITransitionState GetTransitionState(TransitionSystem transitionSystem);
    }
}