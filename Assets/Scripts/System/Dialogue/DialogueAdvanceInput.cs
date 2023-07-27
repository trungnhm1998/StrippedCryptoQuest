using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.System.Dialogue
{
    public class DialogueAdvanceInput : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private Yarn.Unity.LineView _lineView;

        private void OnEnable()
        {
            _inputMediator.NextDialoguePressed += AdvanceDialogue;
        }
        
        private void OnDisable()
        {
            _inputMediator.NextDialoguePressed -= AdvanceDialogue;
        }

        private void AdvanceDialogue()
        {
            _lineView.UserRequestedViewAdvancement();
        }
    }
}