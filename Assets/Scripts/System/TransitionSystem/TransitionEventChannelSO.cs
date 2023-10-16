using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest
{
    [CreateAssetMenu(menuName = "Transition/Transition Event Channel")]
    public class TransitionEventChannelSO : GenericEventChannelSO<AbstractTransition>
    {
#if UNITY_EDITOR
        [field: SerializeReference] public AbstractTransition DebugValue { get; private set; }
#endif
        public override void RaiseEvent(AbstractTransition value)
        {
            base.RaiseEvent(value);
#if UNITY_EDITOR
            if (DebugValue != null) DebugValue = value;
#endif
        }

        protected override void OnRaiseEvent(AbstractTransition value)
        {
            base.OnRaiseEvent(value);
#if UNITY_EDITOR
            if (DebugValue != null) DebugValue = value;
#endif
        }
    }
}