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
        [SerializeField] private Animator _animator;
        [SerializeField] private MoveingDirection _firstMovingDirection;
        [SerializeField] private Transform _startMovingPosition;
        [SerializeField] private Transform _finishMovingPosition;
        [SerializeField] private float _timeDuration;
        [SerializeField] private MoveingDirection _npcFacingDirection;
        [SerializeField] private MoveingDirection _facingToCharacter;
        [SerializeField] private HeroBehaviour _heroBehaviour;
        private MoveingDirection _secondMovingDirection;
        private float _percentComplete = 0f;
        private float _timeElapsed = 0;
        private bool _isMoveForward;
        private bool _isMoving;

        void Start()
        {
            InitializedSetup();
        }

        void Update()
        {
            NPCMovement();
        }

        private void NPCMovement()
        {
            SetFacingDirection(_npcFacingDirection);
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
        private void InitializedSetup()
        {
            _npcFacingDirection = MoveingDirection.None;
            _animator = gameObject.GetComponent<Animator>();
            _animator.runtimeAnimatorController = _animatorOverrideController;
            _startMovingPosition.transform.localPosition = gameObject.transform.localPosition;
            _isMoving = true;
            _isMoveForward = true;
            SetFacingDirection(_firstMovingDirection);
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
            Debug.Log("reverse");
            // SetFacingDirection(direction);
        }
        void OnTriggerEnter2D(Collider2D other)
        {
            var characterFacing = new MoveingDirection();
            _isMoving = false;
            if (_heroBehaviour == null && other.CompareTag("Player"))
            {
                _heroBehaviour = other.GetComponentInParent<HeroBehaviour>();
                return;
            }
            else
            {
                switch (_heroBehaviour.FacingDirection)
                {
                    case HeroBehaviour.EFacingDirection.South:
                        _facingToCharacter = MoveingDirection.MoveTop;
                        SetFacingDirection(_facingToCharacter);
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
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            _isMoving = true;
            SetFacingDirection(_npcFacingDirection);
        }
    }
}
