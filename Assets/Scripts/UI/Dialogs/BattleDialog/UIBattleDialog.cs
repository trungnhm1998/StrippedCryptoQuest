using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.UI.Dialogs.BattleDialog
{
    public class UIBattleDialog : ModalWindow<UIBattleDialog>
    {
        [Header("Config")]
        [SerializeField] private InputMediatorSO _inputMediator;
        [Tooltip("Set this below 0 when you don't want auto hide")]
        [SerializeField] private float _autoHideDelay;

        [Header("UI")]
        [SerializeField] private Text _dialogText;
        [SerializeField] private GameObject _nextMark;

        [Header("Raise Events")]
        [SerializeField] private VoidEventChannelSO _doneShowDialogEvent;

        [Header("Listen Events")]
        [SerializeField] private VoidEventChannelSO _showNextMarkEventChannel;
        [SerializeField] private VoidEventChannelSO _closeBattleDialogEventChannel;

        private string _message;
        private Coroutine _autoNextCoroutine; 

        private void OnEnable()
        {
            _dialogText.text = "";
            _inputMediator.NextDialoguePressed += NextDialog;
            _showNextMarkEventChannel.EventRaised += ShowNextMark;
            _closeBattleDialogEventChannel.EventRaised += CloseDialog;
        }

        private void OnDisable()
        {
            _inputMediator.NextDialoguePressed -= NextDialog;
            _showNextMarkEventChannel.EventRaised -= ShowNextMark;
            _closeBattleDialogEventChannel.EventRaised -= CloseDialog;
        }

        private void NextDialog()
        {
            if (!_nextMark.activeSelf) return;
            _dialogText.text = "";
            _doneShowDialogEvent.RaiseEvent();
            if (_autoNextCoroutine == null) return;
            StopCoroutine(_autoNextCoroutine);
        }

        private void ShowNextMark()
        {
            _nextMark.SetActive(true);
            if (_autoHideDelay < 0) return;
            _autoNextCoroutine = StartCoroutine(DelayAutoNext(_autoHideDelay));
        }

        private IEnumerator DelayAutoNext(float delay)
        {
            yield return new WaitForSeconds(delay);
            NextDialog();
        }

        private void CloseDialog()
        {
            Close();
        }

        protected override void OnBeforeShow()
        {
            base.OnBeforeShow();
            _nextMark.SetActive(false);
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
            gameObject.SetActive(false);
            Visible = false;
            return this;
        }

        protected override void CheckIgnorableForClose() {}
    }
}
