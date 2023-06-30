using CryptoQuest.Events.UI;
using CryptoQuest.Input;
using CryptoQuest.UI;
using IndiGames.Core.Events.ScriptableObjects.Dialogs;
using UnityEngine;

namespace CryptoQuest.Character
{
    public class DialogManager : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private DialogEventChannelSO _dialogEventSO;

        [SerializeField] private IDialog _speechDialog = NullDialog.Instance;

        private void Awake()
        {
            _speechDialog = GetComponentInChildren<IDialog>();
        }

        private void OnEnable()
        {
            _dialogEventSO.ShowEvent += ShowDialog;
            _dialogEventSO.HideEvent += HideDialog;
        }

        private void OnDisable()
        {
            _dialogEventSO.ShowEvent -= ShowDialog;
            _dialogEventSO.HideEvent -= HideDialog;
        }

        private void ShowDialog(DialogueScriptableObject dialogue)
        {
            _inputMediator.EnableMenuInput();
            _speechDialog.SetData(new SpeechDialogArgs()
            {
                DialogueSO = dialogue
            });
            _speechDialog.Show();
        }

        private void HideDialog()
        {
            _inputMediator.EnableMapGameplayInput();
        }
    }
}