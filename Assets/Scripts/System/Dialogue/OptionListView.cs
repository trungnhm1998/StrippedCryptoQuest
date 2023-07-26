using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Unity;

namespace CryptoQuest.System.Dialogue
{
    public class CQOptionListView : DialogueViewBase
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private OptionView _optionViewPrefab;
        [SerializeField] private GameObject _optionViewContainer;
        [SerializeField] private TextMeshProUGUI _lastLineText;
        [SerializeField] private float _fadeTime = 0.1f;
        [SerializeField] private bool _showUnavailableOptions = false;
        private List<OptionView> _optionViews = new();
        private Action<int> _OnOptionSelected;
        private LocalizedLine _lastSeenLine;

        public void Start()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public void Reset()
        {
            _canvasGroup = GetComponentInParent<CanvasGroup>();
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            _lastSeenLine = dialogueLine;
            onDialogueLineFinished();
        }

        public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
        {
            foreach (var optionView in _optionViews)
            {
                optionView.gameObject.SetActive(false);
            }

            while (dialogueOptions.Length > _optionViews.Count)
            {
                var optionView = CreateNewOptionView();
                optionView.gameObject.SetActive(false);
            }

            RenderOptionView(dialogueOptions);
            RenderLastLineText();
            _OnOptionSelected = onOptionSelected;
            StartCoroutine(Effects.FadeAlpha(_canvasGroup, 0, 1, _fadeTime));
        }

        private void RenderLastLineText()
        {
            if (_lastLineText != null)
            {
                if (_lastSeenLine != null)
                {
                    _lastLineText.gameObject.SetActive(true);
                    _lastLineText.text = _lastSeenLine.Text.Text;
                }
                else
                {
                    _lastLineText.gameObject.SetActive(false);
                }
            }
        }

        private void RenderOptionView(DialogueOption[] dialogueOptions)
        {
            int optionViewsCreated = 0;

            for (int i = 0; i < dialogueOptions.Length; i++)
            {
                var optionView = _optionViews[i];
                var option = dialogueOptions[i];

                if (!option.IsAvailable && !_showUnavailableOptions)
                {
                    continue;
                }

                optionView.gameObject.SetActive(true);
                optionView.Option = option;

                if (optionViewsCreated == 0)
                {
                    optionView.Select();
                }

                optionViewsCreated += 1;
            }
        }

        private OptionView CreateNewOptionView()
        {
            var optionView = Instantiate(_optionViewPrefab);
            optionView.transform.SetParent(_optionViewContainer.transform, false);
            optionView.transform.SetAsLastSibling();
            optionView.OnOptionSelected = OptionViewWasSelected;
            _optionViews.Add(optionView);
            return optionView;
        }

        private void OptionViewWasSelected(DialogueOption option)
        {
            StartCoroutine(OptionViewWasSelectedInternal(option));

            IEnumerator OptionViewWasSelectedInternal(DialogueOption selectedOption)
            {
                yield return StartCoroutine(Effects.FadeAlpha(_canvasGroup, 1, 0, _fadeTime));
                _OnOptionSelected(selectedOption.DialogueOptionID);
            }
        }

        public override void DialogueComplete()
        {
            if (_canvasGroup.alpha > 0)
            {
                StopAllCoroutines();
                _lastSeenLine = null;
                _OnOptionSelected = null;
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
                StartCoroutine(Effects.FadeAlpha(_canvasGroup, _canvasGroup.alpha, 0, _fadeTime));
            }
        }
    }
}