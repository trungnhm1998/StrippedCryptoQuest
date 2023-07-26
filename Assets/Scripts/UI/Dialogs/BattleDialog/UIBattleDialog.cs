using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.UI;
using CryptoQuest.Gameplay.Battle;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using DG.Tweening;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.UI.Dialogs.BattleDialog
{
    public class UIBattleDialog : ModalWindow<UIBattleDialog>
    {
        [SerializeField] private InputMediatorSO _inputMediator;

        [Header("UI")]
        [SerializeField] private Text _dialogText;
        [SerializeField] private GameObject _continueMark;

        [Header("Raise Events")]
        [SerializeField] private VoidEventChannelSO _doneShowActionEventChannel;

        [Header("Listen Events")]
        [SerializeField] private VoidEventChannelSO _showNextMarkEventChannel;
        [SerializeField] private VoidEventChannelSO _endActionPhaseEventChannel;

        private string _message;

        private Sequence _showLineSeq;

        private void OnEnable()
        {
            _inputMediator.NextDialoguePressed += NextDialog;
            _showNextMarkEventChannel.EventRaised += ShowNextMark;
            _endActionPhaseEventChannel.EventRaised += CloseDialog;
        }

        private void OnDisable()
        {
            _inputMediator.NextDialoguePressed -= NextDialog;
            _showNextMarkEventChannel.EventRaised -= ShowNextMark;
            _endActionPhaseEventChannel.EventRaised -= CloseDialog;
        }

        private void NextDialog()
        {
            _dialogText.text = "";
            _doneShowActionEventChannel.RaiseEvent();
        }

        private void ShowNextMark()
        {
            _continueMark.SetActive(true);
        }

        private void CloseDialog()
        {
            Close();
        }

        protected override void OnBeforeShow()
        {
            base.OnBeforeShow();
            _continueMark.SetActive(false);
            _inputMediator.EnableDialogueInput();
            _dialogText.text += $"{_message}\n";
        }

        public UIBattleDialog SetDialogue(string dialogueArgs)
        {
            _message = dialogueArgs;
            return this;
        }

        public override UIBattleDialog Close()
        {
            Debug.Log($"who?");
            gameObject.SetActive(false);
            Visible = false;
            return this;
        }

        protected override void CheckIgnorableForClose() {}
    }
}
