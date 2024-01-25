using CryptoQuest.Input;
using CryptoQuest.System.CutsceneSystem;
using CryptoQuest.System.Dialogue.YarnManager;
using UnityEngine;

namespace CryptoQuest.System.Dialogue
{
    public class DialogueAdvanceInput : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private CutsceneInput _cutsceneInput;
        [SerializeField] private LineView _lineView;

        private void OnEnable()
        {
            _inputMediator.NextDialoguePressed += AdvanceDialogue;
            _cutsceneInput.SubmitEvent += AdvanceDialogue;
        }

        private void OnDisable()
        {
            _inputMediator.NextDialoguePressed -= AdvanceDialogue;
            _cutsceneInput.SubmitEvent -= AdvanceDialogue;
        }

        private void AdvanceDialogue()
        {
            _lineView.UserRequestedViewAdvancement();
        }
    }
}