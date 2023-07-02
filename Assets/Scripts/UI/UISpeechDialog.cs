using CryptoQuest.Character;
using CryptoQuest.Gameplay.Quest.Dialogue.ScriptableObject;
using CryptoQuest.Input;
using TMPro;
using UnityEngine;

namespace CryptoQuest.UI
{
    public class SpeechDialogArgs : DialogArgs
    {
        public DialogueScriptableObject DialogueSO;
    }

    public class UISpeechDialog : MonoBehaviour, IDialog
    {
        [SerializeField] private InputMediatorSO _inputMediator;

        [SerializeField] private GameObject _content;
        [SerializeField] private TextMeshProUGUI _dialogLabel;

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

        private void NextDialog()
        {
            _currentDialogueIndex++;
            if (_currentDialogueIndex >= _dialogue.LinesCount)
            {
                Hide();
                return;
            }

            UpdateDialogueWithIndex(_currentDialogueIndex);
        }


        public void Show()
        {
            if (_dialogue.LinesCount == 0)
            {
                Hide();
                return;
            }

            _content.SetActive(true);
            UpdateDialogueWithIndex(_currentDialogueIndex);
        }

        public void Hide()
        {
            Reset();
            _dialogue = null;
            _content.SetActive(false);
            _inputMediator.EnableMapGameplayInput();
        }

        public void SetData(DialogArgs args)
        {
            _dialogue = (args as SpeechDialogArgs)!.DialogueSO;
        }

        private void UpdateDialogueWithIndex(int dialogueIndex)
        {
            _dialogLabel.text = _dialogue.GetLine(dialogueIndex).GetLocalizedString();
        }

        private void Reset()
        {
            _currentDialogueIndex = 0;
            _dialogLabel.text = string.Empty;
        }
    }
}