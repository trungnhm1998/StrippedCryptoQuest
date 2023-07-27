using System;
using System.Collections;
using CryptoQuest.Input;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Yarn.Unity;

namespace CryptoQuest.System.Dialogue
{
    public class LineView : DialogueViewBase
    {
        [SerializeField] private InputMediatorSO _inputMediator;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private bool _useFadeEffect = true;

        [SerializeField]
        [Min(0)]
        private float _fadeInTime = 0.25f;

        [SerializeField]
        [Min(0)]
        private float _fadeOutTime = 0.05f;

        [SerializeField]
        private TextMeshProUGUI _lineText = null;

        [SerializeField]
        private bool _showCharacterNameInLineView = true;

        [SerializeField]
        private TextMeshProUGUI _characterNameText = null;

        [SerializeField]
        private GameObject _characterNameContainer = null;

        [SerializeField]
        private bool _useTypewriterEffect = false;

        [SerializeField]
        private UnityEngine.Events.UnityEvent _onCharacterTyped;

        [SerializeField]
        [Min(0)]
        private float _typewriterEffectSpeed = 0f;

        [FormerlySerializedAs("holdTime")]
        [SerializeField]
        [Min(0)]
        private float _holdTime = 1f;

        [SerializeField]
        private bool _autoAdvance = false;

        private LocalizedLine currentLine = null;

        private Effects.CoroutineInterruptToken currentStopToken = new();

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
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
        }

        private void Reset()
        {
            _canvasGroup = GetComponentInParent<CanvasGroup>();
        }

        public override void DismissLine(Action onDismissalComplete)
        {
            currentLine = null;

            StartCoroutine(DismissLineInternal(onDismissalComplete));
        }

        private IEnumerator DismissLineInternal(Action onDismissalComplete)
        {
            var interactable = _canvasGroup.interactable;
            _canvasGroup.interactable = false;

            if (_useFadeEffect)
            {
                yield return Effects.FadeAlpha(_canvasGroup, 1, 0, _fadeOutTime, currentStopToken);
                currentStopToken.Complete();
            }

            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = interactable;

            if (onDismissalComplete != null)
            {
                onDismissalComplete();
            }
        }

        public override void InterruptLine(LocalizedLine dialogueLine, Action onInterruptLineFinished)
        {
            currentLine = dialogueLine;
            StopAllCoroutines();

            _lineText.gameObject.SetActive(true);
            _canvasGroup.gameObject.SetActive(true);

            int length;

            if (_characterNameText == null)
            {
                if (_showCharacterNameInLineView)
                {
                    _lineText.text = dialogueLine.Text.Text;
                    length = dialogueLine.Text.Text.Length;
                }
                else
                {
                    _characterNameContainer.SetActive(false);
                    _lineText.text = dialogueLine.TextWithoutCharacterName.Text;
                    length = dialogueLine.TextWithoutCharacterName.Text.Length;
                }
            }
            else
            {
                SetCharacterName(dialogueLine);
                length = dialogueLine.TextWithoutCharacterName.Text.Length;
            }

            _lineText.maxVisibleCharacters = length;

            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            onInterruptLineFinished();
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            StopAllCoroutines();

            StartCoroutine(RunLineInternal(dialogueLine, onDialogueLineFinished));
        }

        private IEnumerator RunLineInternal(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            currentLine = dialogueLine;

            yield return PresentLine(dialogueLine);

            currentStopToken.Complete();

            _lineText.maxVisibleCharacters = int.MaxValue;

            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;

            if (_holdTime > 0)
            {
                yield return new WaitForSeconds(_holdTime);
            }

            if (!_autoAdvance)
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


        private IEnumerator PresentLine(LocalizedLine dialogueLine)
        {
            _lineText.gameObject.SetActive(true);
            _canvasGroup.gameObject.SetActive(true);

            if (_characterNameText != null)
            {
                SetCharacterName(dialogueLine);
            }
            else
            {
                _characterNameContainer.SetActive(false);
                if (_showCharacterNameInLineView)
                {
                    _lineText.text = dialogueLine.Text.Text;
                }
                else
                {
                    _lineText.text = dialogueLine.TextWithoutCharacterName.Text;
                }
            }

            if (_useTypewriterEffect)
            {
                _lineText.maxVisibleCharacters = 0;
            }
            else
            {
                _lineText.maxVisibleCharacters = int.MaxValue;
            }

            if (_useFadeEffect)
            {
                yield return StartCoroutine(Effects.FadeAlpha(_canvasGroup, 0, 1, _fadeInTime, currentStopToken));
                if (currentStopToken.WasInterrupted)
                {
                    yield break;
                }
            }

            if (_useTypewriterEffect)
            {
                _canvasGroup.alpha = 1f;
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
                yield return Effects.Typewriter(
                    _lineText,
                    _typewriterEffectSpeed,
                    () => _onCharacterTyped.Invoke(),
                    currentStopToken);
                if (currentStopToken.WasInterrupted)
                {
                    yield break;
                }
            }
        }

        private void SetCharacterName(LocalizedLine dialogueLine)
        {
            _characterNameContainer.SetActive(true);
            _characterNameText.text = dialogueLine.CharacterName;
            if (string.IsNullOrEmpty(dialogueLine.CharacterName))
                _characterNameContainer.SetActive(false);
            _lineText.text = dialogueLine.TextWithoutCharacterName.Text;
        }
    }
}