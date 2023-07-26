using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace CryptoQuest.System.CutsceneSystem.Events
{
    [CreateAssetMenu(menuName = "Crypto Quest/Cutscenes/Events/PlayCutscene")]
    public class PlayCutsceneEvent : ScriptableObject
    {
        public event UnityAction<PlayableDirector> PlayCutsceneRequested;

        public void RaiseEvent(PlayableDirector playableDirector) =>
            PlayCutsceneRequested?.Invoke(playableDirector);
    }
}