using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Input;
using IndiGames.Core.SaveSystem;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using Yarn.Unity;

namespace CryptoQuest
{
    public class DialogTransmiter : MonoBehaviour
    {
        private DialogueRunner _dialogueRunner;
        private InMemoryVariableStorage _yarnVariableStorage;
        [SerializeField] private StringEventChannelSO _onTriggerEndDialogue;
        [SerializeField] private SaveSystemSO _saveSystemSO;
        [SerializeField] private StringEventChannelSO _onTriggerStartDialogue;
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private TableReference _tableReference;
        public List<string> keys = new List<string>();

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

            var stringCollection = LocalizationEditorSettings.GetStringTableCollection("Map");
            foreach (var entry in stringCollection.SharedData.Entries)
            {
                keys.Add(entry.Key);
                var value = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(_tableReference, entry.Key);
                _yarnVariableStorage.SetValue("$" + entry.Key, value.Result);
            }
        }

        private void OnDialogueEnd(string str)
        {
            _onTriggerEndDialogue.RaiseEvent(str);
            _inputMediator.EnableMapGameplayInput();
            Debug.Log("Completed event " + str);
        }

        private void StartDiaglog(string nodeName)
        {
            var stringCollection = LocalizationEditorSettings.GetStringTableCollection("Map");
            foreach (var entry in stringCollection.SharedData.Entries)
            {
                keys.Add(entry.Key);
                var value = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(_tableReference, entry.Key);
                _yarnVariableStorage.SetValue("$" + entry.Key, value.Result);
            }

            _dialogueRunner.StartDialogue(nodeName);
            _inputMediator.EnableMenuInput();
        }
    }
}