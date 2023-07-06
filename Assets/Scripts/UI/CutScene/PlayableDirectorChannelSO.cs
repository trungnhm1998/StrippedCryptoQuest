using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace CryptoQuest.UI.CutScene
{
    [CreateAssetMenu(menuName = "Crypto Quest/CutScene/Playable Director Channel")]
    public class PlayableDirectorChannelSO : ScriptableObject
    {
        public UnityAction<PlayableDirector> OnEventRaised;

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
        }
    }
}