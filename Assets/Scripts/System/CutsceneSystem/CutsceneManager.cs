using CryptoQuest.Quest.Controllers;
using CryptoQuest.System.CutsceneSystem.Events;
using CryptoQuest.System.Dialogue.Managers;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
#if UNITY_EDITOR
using UnityEditor.Timeline;
#endif

namespace CryptoQuest.System.CutsceneSystem
{
    public class CutsceneManager : MonoBehaviour
    {
        public static event UnityAction CutsceneCompleted;
        public static event UnityAction<PlayableDirector> CutsceneFinished;

        [Header("Listening to")] [SerializeField]
        private PlayCutsceneEvent _playCutsceneEvent;

        [SerializeField] private PauseCutsceneEvent _pauseCutsceneEvent;

        [Header("Raise on")] [SerializeField] private UnityEvent _onCutsceneCompleted;

        private TinyMessageSubscriptionToken _pauseCutscene;
        private TinyMessageSubscriptionToken _resumeCutscene;

        /// <summary>
        /// There are multiple directors/cutscenes on a scene, we will try to inject to correct playing director
        /// to this variable at runtime when a cutscene is playing.
        /// </summary>
        private PlayableDirector _currentPlayableDirector;

        private void OnEnable()
        {
            _pauseCutscene = ActionDispatcher.Bind<PauseCutsceneAction>(PauseCutsceneAction);
            _resumeCutscene = ActionDispatcher.Bind<ResumeCutsceneAction>(ResumeCutsceneAction);

            _playCutsceneEvent.PlayCutsceneRequested += PlayCutscene;
            _pauseCutsceneEvent.PauseCutsceneRequested += PauseCutscene;

            YarnSpinnerDialogueManager.PauseTimelineRequested += PauseCutscene;
        }

        private void PauseCutsceneAction(ActionBase _) => PauseCutscene(true);

        private void ResumeCutsceneAction(ActionBase _) => ResumeCutscene();

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_pauseCutscene);
            ActionDispatcher.Unbind(_resumeCutscene);

            _playCutsceneEvent.PlayCutsceneRequested -= PlayCutscene;
            _pauseCutsceneEvent.PauseCutsceneRequested -= PauseCutscene;

            YarnSpinnerDialogueManager.PauseTimelineRequested -= PauseCutscene;
        }

        private CutsceneTrigger _currentCutsceneTrigger;

        private void PlayCutscene(PlayableDirector playableDirector, CutsceneTrigger cutsceneTrigger)
        {
            if (_currentPlayableDirector != null && _currentPlayableDirector.playableGraph.GetRootPlayable(0).IsDone())
            {
                _currentPlayableDirector = null;
            }

            if (playableDirector == _currentPlayableDirector)
            {
                Debug.LogWarning("CutsceneManager::PlayCutscene: Trying to play the same cutscene again.");
                return;
            }

            if (_currentPlayableDirector != null) _currentPlayableDirector.Stop();

            // what happens to the previous cutscene?
            _currentCutsceneTrigger = cutsceneTrigger;
            _currentPlayableDirector = playableDirector;
            _currentPlayableDirector.Play();
            _currentPlayableDirector.stopped += HandleDirectorStopped;
        }

        private void HandleDirectorStopped(PlayableDirector director)
        {
            _currentPlayableDirector.stopped -= HandleDirectorStopped;
            _currentPlayableDirector = null;
            _currentCutsceneTrigger.StopCutscene();
            _currentCutsceneTrigger = null;
            _onCutsceneCompleted.Invoke();
            CutsceneFinished?.Invoke(director);
            CutsceneCompleted?.Invoke();
        }

        private void PauseCutscene(bool isPaused)
        {
            if (_currentPlayableDirector == null)
            {
                Debug.LogWarning(
                    "A request to pause a cutscene was received, but no playable director was previously saved, " +
                    "probably a cutscene was played from editor, and not from the CutsceneTrigger." +
                    "\nAlso could cause by normal dialogue through YarnSpinnerDialogueManager.");

#if UNITY_EDITOR
                _currentPlayableDirector = TimelineEditor.inspectedDirector;
#endif
            }

            if (_currentPlayableDirector)
                _currentPlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(isPaused ? 0 : 1);
        }

        public void ResumeCutscene()
        {
            PauseCutscene(false);
        }
    }
}