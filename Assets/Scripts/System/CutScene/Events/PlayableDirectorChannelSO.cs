using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace CryptoQuest.System.CutScene.Events
{
    [CreateAssetMenu(menuName = "Crypto Quest/Cutscene/Playable Director Channel")]
    public class PlayableDirectorChannelSO : ScriptableObject
    {
        public UnityAction<PlayableDirector> OnEventRaised;

#if UNITY_EDITOR
        public PlayableDirector Value { get; private set; }
#endif

        public void RaiseEvent(PlayableDirector value)
        {
            if (OnEventRaised == null)
            {
                Debug.LogWarning($"A request for {name} has been made, but no one listening.");
                return;
            }

            if (value == null)
            {
                Debug.LogWarning($"A request for {name} has been made, but no value were provided.");
                return;
            }

            OnEventRaised.Invoke(value);

#if UNITY_EDITOR
            Value = value;
#endif
        }
    }
}