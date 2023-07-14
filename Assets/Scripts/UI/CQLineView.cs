using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Input;
using TMPro;
using UnityEngine;
using Yarn.Unity;

namespace CryptoQuest
{
    public class CQLineView : DialogueViewBase
    {
        [SerializeField] private InputMediatorSO _inputMediator;

        [SerializeField]
        internal CanvasGroup canvasGroup;

        [SerializeField]
        internal bool useFadeEffect = true;

        [SerializeField]
        [Min(0)]
        internal float fadeInTime = 0.25f;

        [SerializeField]
        [Min(0)]
        internal float fadeOutTime = 0.05f;

        [SerializeField]
        internal TextMeshProUGUI lineText = null;

        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("showCharacterName")]
        internal bool showCharacterNameInLineView = true;

        [SerializeField]
        internal TextMeshProUGUI characterNameText = null;

        [SerializeField]
        GameObject _characterNameContainer = null;

        [SerializeField]
        internal bool useTypewriterEffect = false;

        [SerializeField]
        internal UnityEngine.Events.UnityEvent onCharacterTyped;

        [SerializeField]
        [Min(0)]
        internal float typewriterEffectSpeed = 0f;

        [SerializeField]
        [Min(0)]
        internal float holdTime = 1f;

        [SerializeField]
        internal bool autoAdvance = false;

        LocalizedLine currentLine = null;

        Effects.CoroutineInterruptToken currentStopToken = new Effects.CoroutineInterruptToken();

        private void OnEnable()
        {
            _inputMediator.MenuConfirmedEvent += Continue;
        }

        private void OnDisable()
        {
            _inputMediator.MenuConfirmedEvent -= Continue;
        }

        public void Continue()
        {
            OnContinueClicked();
        }

        private void Awake()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }

        private void Reset()
        {
            canvasGroup = GetComponentInParent<CanvasGroup>();
        }

        public override void DismissLine(Action onDismissalComplete)
        {
            currentLine = null;

            StartCoroutine(DismissLineInternal(onDismissalComplete));
        }

        private IEnumerator DismissLineInternal(Action onDismissalComplete)
        {
            var interactable = canvasGroup.interactable;
            canvasGroup.interactable = false;

            if (useFadeEffect)
            {
                yield return StartCoroutine(Effects.FadeAlpha(canvasGroup, 1, 0, fadeOutTime, currentStopToken));
                currentStopToken.Complete();
            }

            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = interactable;

            if (onDismissalComplete != null)
            {
                onDismissalComplete();
            }
        }

        public override void InterruptLine(LocalizedLine dialogueLine, Action onInterruptLineFinished)
        {
            currentLine = dialogueLine;
            StopAllCoroutines();

            lineText.gameObject.SetActive(true);
            canvasGroup.gameObject.SetActive(true);

            int length;

            if (characterNameText == null)
            {
                if (showCharacterNameInLineView)
                {
                    lineText.text = dialogueLine.Text.Text;
                    length = dialogueLine.Text.Text.Length;
                }
                else
                {
                    _characterNameContainer.SetActive(false);
                    lineText.text = dialogueLine.TextWithoutCharacterName.Text;
                    length = dialogueLine.TextWithoutCharacterName.Text.Length;
                }
            }
            else
            {
                _characterNameContainer.SetActive(true);
                characterNameText.text = dialogueLine.CharacterName;
                if (string.IsNullOrEmpty(dialogueLine.CharacterName))
                    _characterNameContainer.SetActive(false);
                lineText.text = dialogueLine.TextWithoutCharacterName.Text;
                length = dialogueLine.TextWithoutCharacterName.Text.Length;
            }

            lineText.maxVisibleCharacters = length;

            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            onInterruptLineFinished();
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            StopAllCoroutines();

            StartCoroutine(RunLineInternal(dialogueLine, onDialogueLineFinished));
        }

        private IEnumerator RunLineInternal(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            IEnumerator PresentLine()
            {
                lineText.gameObject.SetActive(true);
                canvasGroup.gameObject.SetActive(true);

                if (characterNameText != null)
                {
                    _characterNameContainer.SetActive(true);
                    characterNameText.text = dialogueLine.CharacterName;
                    if (string.IsNullOrEmpty(dialogueLine.CharacterName))
                        _characterNameContainer.SetActive(false);
                    lineText.text = dialogueLine.TextWithoutCharacterName.Text;
                }
                else
                {
                    _characterNameContainer.SetActive(false);
                    if (showCharacterNameInLineView)
                    {
                        lineText.text = dialogueLine.Text.Text;
                    }
                    else
                    {
                        lineText.text = dialogueLine.TextWithoutCharacterName.Text;
                    }
                }

                if (useTypewriterEffect)
                {
                    lineText.maxVisibleCharacters = 0;
                }
                else
                {
                    lineText.maxVisibleCharacters = int.MaxValue;
                }

                if (useFadeEffect)
                {
                    yield return StartCoroutine(Effects.FadeAlpha(canvasGroup, 0, 1, fadeInTime, currentStopToken));
                    if (currentStopToken.WasInterrupted)
                    {
                        yield break;
                    }
                }

                if (useTypewriterEffect)
                {
                    canvasGroup.alpha = 1f;
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                    yield return StartCoroutine(
                        Effects.Typewriter(
                            lineText,
                            typewriterEffectSpeed,
                            () => onCharacterTyped.Invoke(),
                            currentStopToken
                        )
                    );
                    if (currentStopToken.WasInterrupted)
                    {
                        yield break;
                    }
                }
            }

            currentLine = dialogueLine;

            yield return StartCoroutine(PresentLine());

            currentStopToken.Complete();

            lineText.maxVisibleCharacters = int.MaxValue;

            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;

            if (holdTime > 0)
            {
                yield return new WaitForSeconds(holdTime);
            }

            if (autoAdvance == false)
            {
                yield break;
            }

            onDialogueLineFinished();
        }

        /// <inheritdoc/>
        public override void UserRequestedViewAdvancement()
        {
            if (currentLine == null)
            {
                return;
            }

            if (currentStopToken.CanInterrupt)
            {
                currentStopToken.Interrupt();
            }
            else
            {
                requestInterrupt?.Invoke();
            }
        }

        public void OnContinueClicked()
        {
            UserRequestedViewAdvancement();
        }


        public override void DialogueComplete()
        {
            if (currentLine != null)
            {
                currentLine = null;
                StartCoroutine(DismissLineInternal(null));
            }
        }
    }
}