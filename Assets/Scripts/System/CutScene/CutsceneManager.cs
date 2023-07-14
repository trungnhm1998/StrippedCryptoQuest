using CryptoQuest.Input;
using CryptoQuest.System.Cutscene.Events;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.System.Cutscene
{
    public class CutsceneManager : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;

        [Header("Listening to")]
        [SerializeField] private PlayableDirectorChannelSO _onPlayCutsceneEvent;

        [SerializeField] private VoidEventChannelSO _onPauseTimelineEvent;
        [SerializeField] private VoidEventChannelSO _onResumeTimelineEvent;

        [Header("Option")]
        [SerializeField] private PlayableDirector _director;

        private void OnEnable()
        {
            _onPlayCutsceneEvent.OnEventRaised += OnPlayCutsceneEventRaised;
            _onPauseTimelineEvent.EventRaised += PauseTimeline;
            _onResumeTimelineEvent.EventRaised += ResumeTimeline;
        }

        private void OnDisable()
        {
            _onPlayCutsceneEvent.OnEventRaised -= OnPlayCutsceneEventRaised;
            _onPauseTimelineEvent.EventRaised -= PauseTimeline;
            _onResumeTimelineEvent.EventRaised -= ResumeTimeline;
        }

        private void OnPlayCutsceneEventRaised(PlayableDirector value)
        {
            _inputMediator.EnableDialogueInput();

            _director = value;
            _director.Play();

            _director.stopped += HandleDirectorStopped;
        }

        private void HandleDirectorStopped(PlayableDirector obj) => EndCutScene();

        private void EndCutScene() => _inputMediator.EnableMapGameplayInput();

        private void PauseTimeline() => _director.playableGraph.GetRootPlayable(0).SetSpeed(0);

        private void ResumeTimeline() => _director.playableGraph.GetRootPlayable(0).SetSpeed(1);
    }
}