using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Character.MonoBehaviours;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest
{
    public class NpcMovement : MonoBehaviour
    {
        public enum MoveingDirection
        {
            MoveDown = 0,
            MoveLeft = 1,
            MoveTop = 2,
            MoveRight = 3,
            None = 4
        }

        [SerializeField] private AnimatorOverrideController _animatorOverrideController;
        [SerializeField] private MoveingDirection _firstMovingDirection;
        [SerializeField] private Transform _startMovingPosition;
        [SerializeField] private Transform _finishMovingPosition;
        [SerializeField] private float _timeDuration;
        private MoveingDirection _npcFacingDirection;
        private HeroBehaviour _heroBehaviour;
        private Animator _animator;
        private float _percentComplete = 0f;
        private float _timeElapsed = 0;
        private bool _isMoveForward;
        private bool _isMoving;

        void Start()
        {
            _isMoving = true;
            _isMoveForward = true;
            _npcFacingDirection = MoveingDirection.None;
            _animator = gameObject.GetComponent<Animator>();
            _animator.runtimeAnimatorController = _animatorOverrideController;
            _startMovingPosition.transform.localPosition = gameObject.transform.localPosition;
            SetFacingDirection(_firstMovingDirection);
        }

        void Update()
        {
            if (_isMoving)
            {
                _animator.SetBool("IsWalking", true);
                if (_isMoveForward)
                {
                    NPCMovingForward();
                }
                else
                {
                    NPCMovingBackward();
                }
            }
            else
            {
                _animator.SetBool("IsWalking", false);
            }
        }
        private void NPCMovingForward()
        {
            _percentComplete += Time.deltaTime;
            float percentageCompleted = _percentComplete / _timeDuration;
            gameObject.transform.localPosition = Vector2.Lerp(_startMovingPosition.localPosition,
                _finishMovingPosition.localPosition, percentageCompleted);
            if (_percentComplete > _timeDuration)
            {
                _isMoveForward = false;
                _percentComplete = 0;
                ReverseFacingDirection(_firstMovingDirection);
            }
        }
        private void NPCMovingBackward()
        {
            _percentComplete += Time.deltaTime;
            float percentageCompleted = _percentComplete / _timeDuration;
            gameObject.transform.localPosition = Vector2.Lerp(_finishMovingPosition.localPosition,
                _startMovingPosition.localPosition, percentageCompleted);
            if (_percentComplete > _timeDuration)
            {
                _isMoveForward = true;
                _percentComplete = 0;
                ReverseFacingDirection(_npcFacingDirection);
            }
        }

        private void SetFacingDirection(MoveingDirection direction)
        {
            _animator.SetFloat("AnimVelocityX", 0);
            _animator.SetFloat("AnimVelocityY", 0);
            _npcFacingDirection = direction;
            switch (_npcFacingDirection)
            {
                case MoveingDirection.MoveDown:
                    _animator.SetFloat("AnimVelocityY", -1);
                    break;
                case MoveingDirection.MoveLeft:
                    _animator.SetFloat("AnimVelocityX", -1);
                    break;
                case MoveingDirection.MoveTop:
                    _animator.SetFloat("AnimVelocityY", 1);
                    break;
                case MoveingDirection.MoveRight:
                    _animator.SetFloat("AnimVelocityX", 1);
                    break;
            }
        }

        private void ReverseFacingDirection(MoveingDirection direction)
        {
            switch (direction)
            {
                case MoveingDirection.MoveDown:
                    _npcFacingDirection = MoveingDirection.MoveTop;
                    break;
                case MoveingDirection.MoveLeft:
                    _npcFacingDirection = MoveingDirection.MoveRight;
                    break;
                case MoveingDirection.MoveTop:
                    _npcFacingDirection = MoveingDirection.MoveDown;
                    break;
                case MoveingDirection.MoveRight:
                    _npcFacingDirection = MoveingDirection.MoveLeft;
                    break;
            }
            SetFacingDirection(_npcFacingDirection);
        }
        void OnTriggerEnter2D(Collider2D other)
        {
            _isMoving = false;
            if (_heroBehaviour == null && other.CompareTag("Player"))
            {
                _heroBehaviour = other.GetComponentInParent(typeof(HeroBehaviour)) as HeroBehaviour;
            }
            switch (_heroBehaviour.FacingDirection)
            {
                case HeroBehaviour.EFacingDirection.South:
                    SetFacingDirection(MoveingDirection.MoveTop);
                    break;
                case HeroBehaviour.EFacingDirection.West:
                    SetFacingDirection(MoveingDirection.MoveRight);
                    break;
                case HeroBehaviour.EFacingDirection.North:
                    SetFacingDirection(MoveingDirection.MoveDown);
                    break;
                case HeroBehaviour.EFacingDirection.East:
                    SetFacingDirection(MoveingDirection.MoveLeft);
                    break;
            }
            SetFacingDirection(_npcFacingDirection);
        }

        void OnTriggerExit2D(Collider2D other)
        {
            _isMoving = true;
            if (_isMoveForward)
            {
                SetFacingDirection(_firstMovingDirection);
            }
            else
            {
                ReverseFacingDirection(_firstMovingDirection);
            }
        }
    }
}
