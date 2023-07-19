using CryptoQuest.Gameplay.Quest.Dialogue.ScriptableObject;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Dialogs.Dialogue
{
    public class UIDialogue : ModalWindow<UIDialogue>
    {
        [Header("Child Components")]
        [SerializeField] private InputMediatorSO _inputMediator;

        [SerializeField] private LocalizeStringEvent _dialogueLabel;
        [SerializeField] private GameObject _npcNameTag;
        [SerializeField] private Text _npcName;

        private DialogueScriptableObject _dialogue;
        private int _currentDialogueIndex = 0;

        private void OnEnable()
        {
            _inputMediator.NextDialoguePressed += NextDialog;
        }

        private void OnDisable()
        {
            _inputMediator.NextDialoguePressed -= NextDialog;
        }

        protected override void OnBeforeShow()
        {
            _inputMediator.EnableDialogueInput();
            DisplayNPCName();
            UpdateDialogueWithIndex(_currentDialogueIndex);
        }

        private void NextDialog()
        {
            _currentDialogueIndex++;
            if (_currentDialogueIndex >= _dialogue.LinesCount)
            {
                Close();
                return;
            }

            UpdateDialogueWithIndex(_currentDialogueIndex);
        }

        private void UpdateDialogueWithIndex(int dialogueIndex)
        {
            _dialogueLabel.StringReference = _dialogue.GetLine(dialogueIndex);
        }

        public UIDialogue SetDialogue(DialogueScriptableObject dialogueArgs)
        {
            _dialogue = dialogueArgs;
            return this;
        }

        public override UIDialogue Close()
        {
            _inputMediator.EnableMapGameplayInput();
            gameObject.SetActive(false);
            return base.Close();
        }

        protected override void CheckIgnorableForClose() { }

        private void DisplayNPCName()
        {
            if (_dialogue.NPCName == null)
                _npcNameTag.SetActive(false);
            else
                _npcName.text = _dialogue.NPCName;
        }
    }
}