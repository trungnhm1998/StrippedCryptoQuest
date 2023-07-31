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

        private static readonly int ReactionAnimStateName = Animator.StringToHash("Reaction");
        private static readonly int EmptyStateName = Animator.StringToHash("Empty");

        private float _reactionAnimationLength;
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
            _animator.Play(ReactionAnimStateName, -1, 0);
            _hideCoroutine = StartCoroutine(CoAutoHideReaction());
        }

        private IEnumerator CoAutoHideReaction()
        {
            yield return new WaitForSeconds(_showDuration + _hideAfterSeconds);
            _emoteSpriteRenderer.sprite = null;
            _animator.Play(EmptyStateName);
            yield return new WaitForSeconds(DELAY_BEFORE_RAISE_EVENT);

            ReactionHidden?.Invoke();
        }
    }
}