using System;
using CryptoQuest.Character.Behaviours;
using DG.Tweening;
using UnityEngine;

namespace CryptoQuest.Character.MonoBehaviours
{
    [RequireComponent(typeof(Collider2D), typeof(MovementBehaviour), typeof(FacingBehaviour))]
    /// <summary>
    /// Set trigger collder to auto move to position
    /// </summary>
    public class AutoMoveToPosition : MonoBehaviour
    {
        [SerializeField] private MovementBehaviour _movementBehaviour;
        [SerializeField] private FacingBehaviour _facingBehaviour;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private float _autoMoveDuration = 0.5f;
        private readonly int _idleClip = Animator.StringToHash("Idles");

        public bool IsAutoMoving { get; private set; }

        private void OnValidate()
        {
            _collider = GetComponent<Collider2D>();
            _movementBehaviour = GetComponent<MovementBehaviour>();
            _facingBehaviour = GetComponent<FacingBehaviour>();
        }

        public void AutoMoveTo(Vector3 position, Action onComplete = null)
        {
            DOTween.Kill(transform);
            _movementBehaviour.StopMovement();
            SetAutoMoving(true);
            transform.DOMove(position, _autoMoveDuration).OnComplete(() =>
            {
                SetAutoMoving(false);
                onComplete?.Invoke();
            });
        }

        private void SetAutoMoving(bool isAutoMoving)
        {
            IsAutoMoving = isAutoMoving;
            _movementBehaviour.enabled = !isAutoMoving;
            _facingBehaviour.enabled = !isAutoMoving;
            _collider.isTrigger = isAutoMoving;
        }
    }
}
