using CryptoQuest.Gameplay.Cutscenes.Events;
using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.Gameplay.Cutscenes
{
    public class CutsceneManager : MonoBehaviour
    {
        [Header("Listening to")]
        [SerializeField] private PlayCutsceneEvent _playCutsceneEvent;
        [SerializeField] private PauseCutsceneEvent _pauseCutsceneEvent;

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
            _pauseCutsceneEvent.PauseCutsceneRequested += PauseCutscene;
        }

        private void PlayCutscene(PlayableDirector playableDirector)
        {
            // what happens to the previous cutscene?
            _currentPlayableDirector = playableDirector;
            _currentPlayableDirector.Play();
        }

        private void PauseCutscene(bool isPaused)
        {
            _currentPlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(isPaused ? 0 : 1);
        }
    }
}