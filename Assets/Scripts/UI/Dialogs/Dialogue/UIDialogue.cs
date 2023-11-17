using CryptoQuest.Gameplay.Quest.Dialogue;
using CryptoQuest.Input;
using Input;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace CryptoQuest.UI.Dialogs.Dialogue
{
    public class UIDialogue : ModalWindow<UIDialogue>
    {
        [Header("Child Components")]
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private LocalizeStringEvent _dialogueText;
        [SerializeField] private LocalizeStringEvent _npcNameText;
        [SerializeField] private GameObject _npcNameTag;

        private IDialogueDef _dialogue;
        private int _currentDialogueIndex = 0;

        public override bool Visible { get; set; }

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
            _dialogueText.StringReference = _dialogue.GetLine(dialogueIndex);
        }

        public UIDialogue SetDialogue(IDialogueDef dialogueArgs)
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
            if (_dialogue.SpeakerName.IsEmpty)
                _npcNameTag.SetActive(false);
            else
                _npcNameText.StringReference = _dialogue.SpeakerName;
        }
    }
}