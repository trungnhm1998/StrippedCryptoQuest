using CryptoQuest.Events;
using CryptoQuest.Input;
using CryptoQuest.SaveSystem;
using CryptoQuest.System.SaveSystem;
using UnityEngine;
using Yarn.Unity;

namespace CryptoQuest.System.Dialogue
{
    public class DialogTransmiter : MonoBehaviour
    {
        private DialogueRunner _dialogueRunner;
        private InMemoryVariableStorage _yarnVariableStorage;
        [SerializeField] private StringEventChannelSO _onTriggerEndDialogue;
        [SerializeField] private StringEventChannelSO _onTriggerStartDialogue;
        [SerializeField] private InputMediatorSO _inputMediator;

        private void Awake()
        {
            _dialogueRunner = FindObjectOfType<DialogueRunner>();
            _yarnVariableStorage = FindObjectOfType<InMemoryVariableStorage>();
            _dialogueRunner.AddCommandHandler<string>("EndDialogue", OnDialogueEnd);
        }

        private void OnEnable() => _onTriggerStartDialogue.EventRaised += StartDialog;

        private void OnDisable() => _onTriggerStartDialogue.EventRaised -= StartDialog;

        private void Start() => _yarnVariableStorage.SetValue("$playerName", ServiceProvider.GetService<SaveSystemSO>().PlayerName);

        private void OnDialogueEnd(string str)
        {
            _onTriggerEndDialogue.RaiseEvent(str);
            _inputMediator.EnableMapGameplayInput();
        }

        private void StartDialog(string nodeName)
        {
            _dialogueRunner.StartDialogue(nodeName);
            _inputMediator.EnableMenuInput();
        }
    }
}