using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay;
using CryptoQuest.Input;
using CryptoQuest.System.CutsceneSystem.Events;
using CryptoQuest.System.Dialogue.Events;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

namespace CryptoQuest.System.Dialogue.Managers
{
    public class YarnSpinnerDialogueManager : MonoBehaviour
    {
        public static Action<string> PlayDialogueRequested;

        private static List<YarnSpinnerDialogueManager> _systems = new List<YarnSpinnerDialogueManager>();

        /// <summary>
        /// Little hack to support cross scene, and make sure there always one active instance.
        /// </summary>
        public static YarnSpinnerDialogueManager Instance
        {
            get => _systems.Count > 0 ? _systems[0] : null;
            set
            {
                var index = _systems.IndexOf(value);

                if (index > 0)
                {
                    _systems.RemoveAt(index);
                    _systems.Insert(0, value);
                }
                else if (index < 0)
                {
                    Debug.LogError($"Failed setting YarnSpinnerDialogueManager.Instance to unknown system " + value);
                }
            }
        }

        /// <summary>
        /// TODO: Move to a class that manages <see cref="YarnSpinnerDialogueManager"/> and <see cref="DialogueManager"/>
        /// </summary>
        [SerializeField] private GameStateSO _gameState;

        [SerializeField] private InputMediatorSO _inputMediator;

        [Header("UI")]
        [SerializeField] private DialogueRunner _dialogueRunner;

        [Header("Listen to")]
        [SerializeField] private PlayDialogueEvent _playDialogueEventEvent;

        [Header("Raise on")]
        [SerializeField] private VoidEventChannelSO _dialogueCompletedEventChannelSO;

        [SerializeField] private PauseCutsceneEvent _pauseCutsceneEvent;

        [SerializeField] private UnityEvent<string> _onReactionShowed;

        [SerializeField] private UnityEvent _onDialogueCompleted;

        private Yarn.Dialogue Dialogue => _dialogueRunner.Dialogue;

        private void OnEnable()
        {
            _systems.Add(this);
            PlayDialogueRequested += ShowDialogue;
            _playDialogueEventEvent.PlayDialogueRequested += ShowDialogue;
        }

        private void OnDisable()
        {
            PlayDialogueRequested -= ShowDialogue;
            _playDialogueEventEvent.PlayDialogueRequested -= ShowDialogue;
            _systems.Remove(this);
        }

        private void ShowDialogue(string yarnNodeName)
        {
            if (Dialogue.IsActive)
            {
                Debug.LogWarning(
                    "YarnSpinnerDialogueManager::ShowDialogue: Try run show dialogue while the previous still running.");
                _pauseCutsceneEvent.RaiseEvent(true);
                return;
            }

            Debug.Log($"YarnSpinnerDialogueManager::ShowDialogue: yarnNodeName[{yarnNodeName}]");
            _gameState.UpdateGameState(EGameState.Dialogue);
            _inputMediator.EnableDialogueInput();
            _dialogueRunner.StartDialogue(yarnNodeName);
        }

        public void DialogueCompleted()
        {
            _gameState.RevertGameState();

            if (_gameState.CurrentGameState is EGameState.Field or EGameState.Battle)
                _inputMediator.EnableMapGameplayInput();

            // support cross scene
            if (_dialogueCompletedEventChannelSO != null) _dialogueCompletedEventChannelSO.RaiseEvent();
            _onDialogueCompleted.Invoke();
        }

        /// <summary>
        /// Because of this method I have to use singleton or else the system use Find with name in a large scene
        /// </summary>
        /// <param name="reactionName"></param>
        [YarnCommand("react")]
        public static void React(string reactionName)
        {
            if (Instance == null)
            {
                Debug.LogWarning("YarnSpinnerDialogueManager::React: _instance is null");
                return;
            }

            Instance.ShowReaction(reactionName);
        }

        private void ShowReaction(string reactionName)
        {
            Debug.Log($"YarnSpinnerDialogueManager::ShowReaction: reactionName[{reactionName}]");
            _onReactionShowed?.Invoke(reactionName);
        }
    }
}