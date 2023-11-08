using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Events;
using CryptoQuest.Gameplay;
using CryptoQuest.Input;
using CryptoQuest.System.CutsceneSystem;
using CryptoQuest.System.CutsceneSystem.Events;
using CryptoQuest.System.Dialogue.Events;
using CryptoQuest.System.Dialogue.YarnManager;
using CryptoQuest.System.SaveSystem;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

namespace CryptoQuest.System.Dialogue.Managers
{
    public class YarnSpinnerDialogueManager : MonoBehaviour
    {
        public static Action<string> DialogueCompletedEvent;
        public static Action<string> PlayDialogueRequested;
        public static Action<bool> PauseTimelineRequested;
        public static Action<YarnProjectConfigSO> YarnProjectRequested;
        public static bool IsYarnTableLoaded;

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
        [SerializeField] private SaveSystemSO _saveSystem;

        [Header("UI")] [SerializeField] private DialogueRunner _dialogueRunner;

        [Header("Listen to")] [SerializeField] private PlayDialogueEvent _playDialogueEventEvent;

        [Header("Raise on")] [SerializeField] private VoidEventChannelSO _dialogueCompletedEventChannelSO;

        [SerializeField] private PauseCutsceneEvent _pauseCutsceneEvent;
        [SerializeField] private YarnProjectConfigEvent _projectConfigEvent;

        [SerializeField] private UnityEvent<string> _onReactionShowed;

        [SerializeField] private UnityEvent _onDialogueCompleted;
        [SerializeField] private StringEventChannelSO _onCompleteQuestEventChannelSO;
        private string _currentYarnNode;

        private Yarn.Dialogue Dialogue => _dialogueRunner.Dialogue;

        private void Awake() => _dialogueRunner.AddFunction("GetPlayerName", GetPlayerName);

        private void OnEnable()
        {
            _systems.Add(this);
            PlayDialogueRequested += ShowDialogue;
            YarnProjectRequested += ConfigureYarnProject;
            _playDialogueEventEvent.PlayDialogueRequested += ShowDialogue;
        }

        private void OnDisable()
        {
            PlayDialogueRequested -= ShowDialogue;
            YarnProjectRequested -= ConfigureYarnProject;
            _playDialogueEventEvent.PlayDialogueRequested -= ShowDialogue;
            _systems.Remove(this);
        }

        private void ConfigureYarnProject(YarnProjectConfigSO yarnProject)
        {
            IsYarnTableLoaded = false;
            _projectConfigEvent.ConfigureYarnProject(yarnProject);
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

            _currentYarnNode = yarnNodeName;
            Debug.Log($"YarnSpinnerDialogueManager::ShowDialogue: yarnNodeName[{yarnNodeName}]");
            _gameState.UpdateGameState(EGameState.Dialogue);
            _inputMediator.EnableDialogueInput();
            StartCoroutine(CoRunDialogue(yarnNodeName));
        }

        private IEnumerator CoRunDialogue(string yarnNodeName)
        {
            yield return new WaitUntil(() => IsYarnTableLoaded);
            _dialogueRunner.StartDialogue(yarnNodeName);
        }

        public void DialogueCompleted()
        {
            _gameState.RevertGameState();

            if (_gameState.CurrentGameState is EGameState.Field or EGameState.Battle)
                _inputMediator.EnableMapGameplayInput();

            // support cross scene
            if (_dialogueCompletedEventChannelSO != null) _dialogueCompletedEventChannelSO.RaiseEvent();
            DialogueCompletedEvent?.Invoke(_currentYarnNode);
            _onDialogueCompleted.Invoke();
            _currentYarnNode = "";
        }

        private string GetPlayerName()
        {
#if UNITY_EDITOR
            return string.IsNullOrEmpty(_saveSystem.PlayerName) ? "Abel" : _saveSystem.PlayerName;
#else
            return _saveSystem.PlayerName;
#endif
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

        [YarnCommand("quest")]
        public static void CompleteQuest(string questName)
        {
            if (Instance == null)
            {
                Debug.LogWarning("YarnSpinnerDialogueManager::CompleteQuest: _instance is null");
                return;
            }

            Instance.OnCompleteQuest(questName);
        }

        [YarnCommand("choose")]
        public static void Choose(string choiceId)
        {
            if (Instance == null)
            {
                Debug.LogWarning("YarnSpinnerDialogueManager::Choose: _instance is null");
                return;
            }

            Instance.OnMadeChoice(choiceId);
        }

        private void OnCompleteQuest(string questName)
        {
            Debug.Log($"YarnSpinnerDialogueManager::OnCompleteQuest: questName[{questName}]");
            _onCompleteQuestEventChannelSO.RaiseEvent(questName);
        }

        private void OnMadeChoice(string choiceId)
        {
            Debug.Log($"YarnSpinnerDialogueManager::OnMadeChoice: choiceId[{choiceId}]");
            CutsceneChoiceController.MadeChoice?.Invoke(choiceId);
        }

        private void ShowReaction(string reactionName)
        {
            Debug.Log($"YarnSpinnerDialogueManager::ShowReaction: reactionName[{reactionName}]");
            _onReactionShowed?.Invoke(reactionName);
        }
    }
}