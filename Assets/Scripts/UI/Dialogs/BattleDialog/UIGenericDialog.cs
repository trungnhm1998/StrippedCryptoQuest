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
            _inputMediator.NextDialoguePressed += Hide;
            _inputMediator.EscapeDialoguePressed += Hide;
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

        public UIGenericDialog AppendMessage(string message)
        {
            var newLine = string.IsNullOrEmpty(_dialogText.text) ? "" : "\n";
            _dialogText.text += newLine + message;
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
        public IEnumerator CoAppendMessage(LocalizedString message)
        {
            var messageHandle = message.GetLocalizedStringAsync();
            yield return messageHandle;
            var newLine = string.IsNullOrEmpty(_dialogText.text) ? "" : "\n";
            _dialogText.text += newLine + messageHandle.Result;
        }

        public override void Show() => StartCoroutine(CoShow());

        public IEnumerator CoShow()
        {
            if (_localizedMessage != null)
            {
                yield return CoShowWithLocalizedMessage();
            }

            InternalShow();
        }

        private IEnumerator CoShowWithLocalizedMessage()
        {
            var messageHandle = _localizedMessage.GetLocalizedStringAsync();
            yield return messageHandle;
            _message = messageHandle.Result;
        }

        private void InternalShow()
        {
            _nextMark.SetActive(false);
            _dialogText.text = _message;
            if (_requireInput) _inputMediator.EnableDialogueInput();

            base.Show();

            if (_autoHideDuration > 0)
                Invoke(nameof(Hide), _autoHideDuration);
            _autoHideDuration = 0;
        }

        public override void Hide()
        {
            base.Hide();
            _inputMediator.NextDialoguePressed -= Hide;
            _inputMediator.EscapeDialoguePressed -= Hide;
            _hideCallback?.Invoke();
            _hideCallback = null;

            _requireInput = false;
            Clear();
        }

        private float _autoHideDuration;

        public UIGenericDialog WithAutoHide(float duration)
        {
            _autoHideDuration = duration;
            return this;
        }

        public virtual void Clear()
        {
            _dialogText.text = "";
        }

        private void OnDisable()
        {
            _inputMediator.NextDialoguePressed -= Hide;
            _inputMediator.EscapeDialoguePressed -= Hide;
        }
    }
}