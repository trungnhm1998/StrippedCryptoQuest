using System;
using System.Collections;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace CryptoQuest.UI.Dialogs.BattleDialog
{
    public class UIGenericDialog : AbstractDialog
    {
        [Header("Config")]
        [SerializeField] private InputMediatorSO _inputMediator;

        [Header("UI")]
        [SerializeField] private Text _dialogText;
        [SerializeField] private GameObject _nextMark;
        
        private bool _requireInput;
        public UIGenericDialog RequireInput()
        {
            _requireInput = true;
            return this;
        }

        private string _message;
        public UIGenericDialog WithMessage(string message)
        {
            _message = message;
            return this;
        }

        private LocalizedString _localizedMessage;

        public UIGenericDialog WithMessage(LocalizedString message)
        {
            _localizedMessage = message;
            return this;
        }

        public UIGenericDialog AppendMessage(LocalizedString message)
        {
            StartCoroutine(CoAppendMessage(message));
            return this;
        }

        private Action _hideCallback;

        public UIGenericDialog WithHideCallback(Action hideCallBack)
        {
            _hideCallback = hideCallBack;
            return this;
        }

        /// <summary>
        /// Append new line to the dialog text
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private IEnumerator CoAppendMessage(LocalizedString message)
        {
            var messageHandle = message.GetLocalizedStringAsync();
            yield return messageHandle;
            _dialogText.text += "\n" + messageHandle.Result;
        }

        public override void Show()
        {
            if (_localizedMessage != null)
            {
                StartCoroutine(CoShowWithLocalizedMessage());
                return;
            }

            InternalShow();
        }

        private IEnumerator CoShowWithLocalizedMessage()
        {
            var messageHandle = _localizedMessage.GetLocalizedStringAsync();
            yield return messageHandle;
            _message = messageHandle.Result;
            InternalShow();
        }

        private void InternalShow()
        {
            _nextMark.SetActive(false);
            _dialogText.text = _message;
            _inputMediator.NextDialoguePressed += Hide;
            _inputMediator.DisableAllInput();
            _inputMediator.EnableInputMap("Dialogues");
            base.Show();

            if (_autoHideDuration > 0)
                Invoke(nameof(Hide), _autoHideDuration);
        }

        public override void Hide()
        {
            _inputMediator.NextDialoguePressed -= Hide;
            base.Hide();
            _dialogText.text = "";
            _hideCallback?.Invoke();
        }

        private float _autoHideDuration;

        public UIGenericDialog WithAutoHide(float duration)
        {
            _autoHideDuration = duration;
            return this;
        }
    }
}