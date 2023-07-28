using CryptoQuest.System.CutsceneSystem.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace CryptoQuest.System.CutsceneSystem
{
    public class CutsceneManager : MonoBehaviour
    {
        [Header("Listening to")]
        [SerializeField] private PlayCutsceneEvent _playCutsceneEvent;

        [SerializeField] private PauseCutsceneEvent _pauseCutsceneEvent;
        
        [Header("Raise on")]
        [SerializeField] private UnityEvent _onCutsceneCompleted;

        /// <summary>
        /// There are multiple directors/cutscenes on a scene, we will try to inject to correct playing director
        /// to this variable at runtime when a cutscene is playing.
        /// </summary>
        private PlayableDirector _currentPlayableDirector;

        private void OnEnable()
        {
            _playCutsceneEvent.PlayCutsceneRequested += PlayCutscene;
            _pauseCutsceneEvent.PauseCutsceneRequested += PauseCutscene;
        }

        private void OnDisable()
        {
            _playCutsceneEvent.PlayCutsceneRequested -= PlayCutscene;
            _pauseCutsceneEvent.PauseCutsceneRequested -= PauseCutscene;
        }

        private void PlayCutscene(PlayableDirector playableDirector)
        {
            if (playableDirector == _currentPlayableDirector)
            {
                Debug.LogWarning("CutsceneManager::PlayCutscene: Trying to play the same cutscene again.");
                return;
            }

            if (_currentPlayableDirector != null) _currentPlayableDirector.Stop();

            // what happens to the previous cutscene?
            _currentPlayableDirector = playableDirector;
            _currentPlayableDirector.Play();
            _currentPlayableDirector.stopped += HandleDirectorStopped;
        }

        private void HandleDirectorStopped(PlayableDirector director)
        {
            _currentPlayableDirector.stopped -= HandleDirectorStopped;
            _currentPlayableDirector = null;
            _onCutsceneCompleted.Invoke();
        }

        private void PauseCutscene(bool isPaused)
        {
            if (_currentPlayableDirector == null)
            {
                Debug.LogWarning(
                    "A request to pause a cutscene was received, but no playable director was previously saved, " +
                    "probably a cutscene was played from editor, and not from the CutsceneTrigger.");
                return;
            }

            _currentPlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(isPaused ? 0 : 1);
        }

        public void ResumeCutscene()
        {
            PauseCutscene(false);
        }
    }
}