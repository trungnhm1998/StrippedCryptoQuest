using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Character.Reaction
{
    public class ReactionController : MonoBehaviour
    {
        public event UnityAction ShowingReaction;
        public event UnityAction ReactionHidden;

        [SerializeField] private float _hideAfterSeconds = 1.5f;
        [SerializeField] private float _showDuration = 0.5f;
        [SerializeField] private SpriteRenderer _emoteSpriteRenderer;
        [SerializeField] private Animator _animator;

        /// <summary>
        /// This is a design choice by planner
        /// </summary>
        private const float DELAY_BEFORE_RAISE_EVENT = 0.5f;

        private static readonly int HideReactionTrigger = Animator.StringToHash("Hide");
        private static readonly int ShowReactionTrigger = Animator.StringToHash("Show");

        private Coroutine _hideCoroutine;

        public void ShowReaction(Reaction reaction, float hideAfterSeconds)
        {
            if (hideAfterSeconds >= 0f)
                _hideAfterSeconds = hideAfterSeconds;
            ShowReaction(reaction);
        }

        public void ShowReaction(Reaction reaction)
        {
            if (reaction == null)
            {
                Debug.LogWarning($"Trying to show null reaction on {gameObject.name}");
                return;
            }

            if (_hideCoroutine != null) StopCoroutine(_hideCoroutine);

            ShowingReaction?.Invoke();

            _emoteSpriteRenderer.sprite = reaction.ReactionIcon;
            _animator.ResetTrigger(HideReactionTrigger);
            _animator.SetTrigger(ShowReactionTrigger);
            _hideCoroutine = StartCoroutine(CoAutoHideReaction());
        }

        private IEnumerator CoAutoHideReaction()
        {
            yield return new WaitForSeconds(_showDuration + _hideAfterSeconds);
            _emoteSpriteRenderer.sprite = null;
            _animator.ResetTrigger(ShowReactionTrigger);
            _animator.SetTrigger(HideReactionTrigger);
            yield return new WaitForSeconds(DELAY_BEFORE_RAISE_EVENT);

            ReactionHidden?.Invoke();
        }
    }
}