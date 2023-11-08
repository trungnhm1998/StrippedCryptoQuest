using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Unity;

namespace CryptoQuest.System.Dialogue.YarnManager
{
    /// <summary>
    /// This is plugin code for customizing for show playername in dialogue option
    /// </summary>
    public class OptionsListView : DialogueViewBase
    {
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] CanvasGroup mainCanvasGroup;

        [SerializeField] OptionView optionViewPrefab;

        [SerializeField] TextMeshProUGUI lastLineText;

        [SerializeField] TextMeshProUGUI playerNameText;

        [SerializeField] float fadeTime = 0.1f;

        [SerializeField] bool showUnavailableOptions = false;

        // A cached pool of OptionView objects so that we can reuse them
        List<OptionView> optionViews = new List<OptionView>();

        // The method we should call when an option has been selected.
        Action<int> OnOptionSelected;

        // The line we saw most recently.
        LocalizedLine lastSeenLine;

        public void Start()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            mainCanvasGroup.alpha = 0;
            mainCanvasGroup.interactable = false;
            mainCanvasGroup.blocksRaycasts = false;
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            // Change this line for show playername in dialogue option
            playerNameText.text = dialogueLine.CharacterName;

            // Don't do anything with this line except note it and
            // immediately indicate that we're finished with it. RunOptions
            // will use it to display the text of the previous line.
            lastSeenLine = dialogueLine;
            onDialogueLineFinished();
        }

        public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
        {
            // Hide all existing option views
            foreach (var optionView in optionViews)
            {
                optionView.gameObject.SetActive(false);
            }

            // If we don't already have enough option views, create more
            while (dialogueOptions.Length > optionViews.Count)
            {
                var optionView = CreateNewOptionView();
                optionView.gameObject.SetActive(false);
            }

            // Set up all of the option views
            int optionViewsCreated = 0;

            for (int i = 0; i < dialogueOptions.Length; i++)
            {
                var optionView = optionViews[i];
                var option = dialogueOptions[i];

                if (option.IsAvailable == false && showUnavailableOptions == false)
                {
                    // Don't show this option.
                    continue;
                }

                optionView.gameObject.SetActive(true);

                optionView.Option = option;

                // The first available option is selected by default
                if (optionViewsCreated == 0)
                {
                    optionView.Select();
                }

                optionViewsCreated += 1;
            }

            // Update the last line, if one is configured
            if (lastLineText != null)
            {
                if (lastSeenLine != null)
                {
                    lastLineText.gameObject.SetActive(true);
                    lastLineText.text = lastSeenLine.TextWithoutCharacterName.Text;
                }
                else
                {
                    lastLineText.gameObject.SetActive(false);
                }
            }

            // Note the delegate to call when an option is selected
            OnOptionSelected = onOptionSelected;

            // Fade it all in
            StartCoroutine(CoFadeAlpha(0, 1));

            /// <summary>
            /// Creates and configures a new <see cref="OptionView"/>, and adds
            /// it to <see cref="optionViews"/>.
            /// </summary>
            OptionView CreateNewOptionView()
            {
                var optionView = Instantiate(optionViewPrefab);
                optionView.transform.SetParent(canvasGroup.transform, false);
                optionView.transform.SetAsLastSibling();

                optionView.OnOptionSelected = OptionViewWasSelected;
                optionViews.Add(optionView);

                return optionView;
            }

            /// <summary>
            /// Called by <see cref="OptionView"/> objects.
            /// </summary>
            void OptionViewWasSelected(DialogueOption option)
            {
                StartCoroutine(OptionViewWasSelectedInternal(option));

                IEnumerator OptionViewWasSelectedInternal(DialogueOption selectedOption)
                {
                    yield return StartCoroutine(CoFadeAlpha(1, 0));
                    OnOptionSelected(selectedOption.DialogueOptionID);
                }
            }
        }

        private IEnumerator CoFadeAlpha(float from, float to)
        {
            yield return Effects.FadeAlpha(mainCanvasGroup, from, to, fadeTime);
            yield return Effects.FadeAlpha(canvasGroup, from, to, fadeTime);
        }

        /// <inheritdoc />
        /// <remarks>
        /// If options are still shown dismisses them.
        /// </remarks>
        public override void DialogueComplete()
        {
            // do we still have any options being shown?
            if (canvasGroup.alpha > 0 && mainCanvasGroup.alpha > 0)
            {
                StopAllCoroutines();
                lastSeenLine = null;
                OnOptionSelected = null;

                mainCanvasGroup.interactable = false;
                mainCanvasGroup.blocksRaycasts = false;

                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;

                StartCoroutine(CoFadeAlpha(canvasGroup.alpha, 0));
            }
        }
    }
}