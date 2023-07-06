using System;
using CryptoQuest.Events.UI;
using CryptoQuest.Gameplay.Quest.Dialogue.ScriptableObject;
using CryptoQuest.Input;
using CryptoQuest.UI.CutScene;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.System.CutScene
{
    public class CutsceneManager : MonoBehaviour
    {
        [SerializeField] private DialogueScriptableObject _dialogue;
        [SerializeField] private InputMediatorSO _inputMediator;

        [Header("Listening to")]
        [SerializeField] private PlayableDirectorChannelSO _onPlayCutsceneEvent;

        [SerializeField] private VoidEventChannelSO _onPauseTimelineEvent;
        [SerializeField] private VoidEventChannelSO _onLineEndedEvent;

        [Header("Raising event")]
        [SerializeField] private DialogEventChannelSO _onShowDialogEvent;

        [Header("Option")]
        [SerializeField] private PlayableDirector _director;

        private bool _isDialoguePlayed;

        private void OnEnable()
        {
            _onPlayCutsceneEvent.OnEventRaised += OnPlayCutsceneEventRaised;
        }

        private void OnPlayCutsceneEventRaised(PlayableDirector value)
        {
            _inputMediator.EnableDialogueInput();

            _director = value;
            _director.Play();
            _director.stopped += HandleDirectorStopped;
        }

        private void HandleDirectorStopped(PlayableDirector obj)
        {
            EndCutScene();
        }

        private void EndCutScene()
        {
            if (_onPlayCutsceneEvent != null) _onPlayCutsceneEvent.OnEventRaised -= OnPlayCutsceneEventRaised;

            _inputMediator.EnableMapGameplayInput();
        }
    }
}