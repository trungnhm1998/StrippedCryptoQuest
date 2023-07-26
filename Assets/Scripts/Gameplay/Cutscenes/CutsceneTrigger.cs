using CryptoQuest.Gameplay.Cutscenes.Events;
using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.Gameplay.Cutscenes
{
    public class CutsceneTrigger : MonoBehaviour
    {
        [SerializeField] private PlayableDirector _playableDirector;

        [Header("Raise on")]
        [SerializeField] private PlayCutsceneEvent _playCutsceneEvent;

        public void PlayCutscene()
        {
            _playCutsceneEvent.RaiseEvent(_playableDirector);
        }
    }
}