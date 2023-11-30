using UnityEngine;

namespace CryptoQuest.System.TransitionSystem
{
    public abstract class AbstractTransition : ScriptableObject
    {
        public abstract ITransitionState GetTransitionState(TransitionSystem transitionSystem);
    }
}