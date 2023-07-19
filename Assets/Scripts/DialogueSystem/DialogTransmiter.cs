using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Events;
using CryptoQuest.Input;
using IndiGames.Core.SaveSystem;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using Yarn.Unity;

namespace CryptoQuest.DialogueSystem
{
    public class DialogTransmiter : MonoBehaviour
    {
        private DialogueRunner _dialogueRunner;
        private InMemoryVariableStorage _yarnVariableStorage;
        [SerializeField] private StringEventChannelSO _onTriggerEndDialogue;
        [SerializeField] private SaveSystemSO _saveSystemSO;
        [SerializeField] private StringEventChannelSO _onTriggerStartDialogue;
        [SerializeField] private InputMediatorSO _inputMediator;

        private void Awake()
        {
            _dialogueRunner = FindObjectOfType<DialogueRunner>();

            _yarnVariableStorage = FindObjectOfType<InMemoryVariableStorage>();
            _dialogueRunner.AddCommandHandler<string>("EndDialogue", OnDialogueEnd);
        }

        private void OnEnable()
        {
            _onTriggerStartDialogue.EventRaised += StartDiaglog;
        }

        private void OnDisable()
        {
            _onTriggerStartDialogue.EventRaised -= StartDiaglog;
        }

        private void Start()
        {
            _yarnVariableStorage.SetValue("$playerName", _saveSystemSO.PlayerName);
        }

        private void OnDialogueEnd(string str)
        {
            _onTriggerEndDialogue.RaiseEvent(str);
            _inputMediator.EnableMapGameplayInput();
        }

        private void StartDiaglog(string nodeName)
        {
            _dialogueRunner.StartDialogue(nodeName);
            _inputMediator.EnableMenuInput();
        }
    }
}