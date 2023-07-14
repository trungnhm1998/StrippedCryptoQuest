using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Input;
using TMPro;
using UnityEngine;
using Yarn.Unity;

namespace CryptoQuest
{
    public class CQOptionListView : DialogueViewBase
    {
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] OptionView optionViewPrefab;
        [SerializeField] GameObject optionViewContainer; 
        [SerializeField] TextMeshProUGUI lastLineText;
        [SerializeField] float fadeTime = 0.1f;
        [SerializeField] bool showUnavailableOptions = false;
        List<OptionView> optionViews = new List<OptionView>();
        Action<int> OnOptionSelected;
        LocalizedLine lastSeenLine;
        
        public void Start()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        public void Reset()
        {
            canvasGroup = GetComponentInParent<CanvasGroup>();
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            lastSeenLine = dialogueLine;
            onDialogueLineFinished();
        }

        public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
        {
            foreach (var optionView in optionViews)
            {
                optionView.gameObject.SetActive(false);
            }

            while (dialogueOptions.Length > optionViews.Count)
            {
                var optionView = CreateNewOptionView();
                optionView.gameObject.SetActive(false);
            }

            int optionViewsCreated = 0;

            for (int i = 0; i < dialogueOptions.Length; i++)
            {
                var optionView = optionViews[i];
                var option = dialogueOptions[i];

                if (option.IsAvailable == false && showUnavailableOptions == false)
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

            if (lastLineText != null)
            {
                if (lastSeenLine != null)
                {
                    lastLineText.gameObject.SetActive(true);
                    lastLineText.text = lastSeenLine.Text.Text;
                }
                else
                {
                    lastLineText.gameObject.SetActive(false);
                }
            }

            OnOptionSelected = onOptionSelected;

            StartCoroutine(Effects.FadeAlpha(canvasGroup, 0, 1, fadeTime));

            OptionView CreateNewOptionView()
            {
                var optionView = Instantiate(optionViewPrefab);
                optionView.transform.SetParent(optionViewContainer.transform, false);
                optionView.transform.SetAsLastSibling();

                optionView.OnOptionSelected = OptionViewWasSelected;
                optionViews.Add(optionView);

                return optionView;
            }

            void OptionViewWasSelected(DialogueOption option)
            {
                StartCoroutine(OptionViewWasSelectedInternal(option));

                IEnumerator OptionViewWasSelectedInternal(DialogueOption selectedOption)
                {
                    yield return StartCoroutine(Effects.FadeAlpha(canvasGroup, 1, 0, fadeTime));
                    OnOptionSelected(selectedOption.DialogueOptionID);
                }
            }
        }

        public override void DialogueComplete()
        {
            if (canvasGroup.alpha > 0)
            {
                StopAllCoroutines();
                lastSeenLine = null;
                OnOptionSelected = null;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;

                StartCoroutine(Effects.FadeAlpha(canvasGroup, canvasGroup.alpha, 0, fadeTime));
            }
        }
    }
}