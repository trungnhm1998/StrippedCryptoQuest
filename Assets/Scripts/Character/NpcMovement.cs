using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Character.Movement;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest
{
    public class NpcMovement : CharacterBehaviour
    {
        [SerializeField] private AnimatorOverrideController _animatorOverrideController;
        [SerializeField] private EFacingDirection _playerFacingDirection = EFacingDirection.South;
        [SerializeField] private Transform _pointAPosition;
        [SerializeField] private Transform _pointBPosition;
        [SerializeField] private Transform _destination;
        [SerializeField] private GameObject _testPoint;
        [SerializeField] private float _speed;
        [SerializeField] private float _delayTime;
        private float _timeElapsed = 0;
        private bool _isMoving;
        private Animator _animatorNpc;
        private Vector2 _characterVelocity;
        private Vector2 sprite;


        void Start()
        {
            _isMoving = true;
            SpriteRenderer _spriteRenderer = GetComponent<SpriteRenderer>();
            sprite = new Vector2(_spriteRenderer.sprite.rect.width * 0.01f / 2, (_spriteRenderer.sprite.rect.height * 0.01f) / 2);

            _testPoint.transform.localPosition = sprite;
            _animatorNpc = _animator;
            _animatorNpc.runtimeAnimatorController = _animatorOverrideController;
            if (_destination == null) _destination = _pointAPosition;
            GetCharacterVelocity();
        }

        void Update()
        {
            Move();
        }

        private void Move()
        {
            if (_isMoving)
                transform.localPosition = Vector2.MoveTowards(transform.localPosition, _destination.localPosition, _speed * Time.deltaTime);

            if (gameObject.transform.localPosition == _destination.localPosition)
            {
                _isMoving = false;
                _timeElapsed += Time.deltaTime;
                if (_timeElapsed > _delayTime)
                {
                    SwapDestination();
                    _timeElapsed = 0;
                }
            }
            _animatorNpc.SetBool(AnimIsWalking, _isMoving);
        }

        private void SwapDestination()
        {
            if (_destination == _pointAPosition)
            {
                _destination = _pointBPosition;
            }
            else
            {
                _destination = _pointAPosition;
            }
            _isMoving = true;
            GetCharacterVelocity();
        }

        private void GetCharacterVelocity()
        {
            _characterVelocity = _destination.localPosition - gameObject.transform.localPosition;
            SetFacingDirection(_characterVelocity.normalized);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _isMoving = IsWalking = false;
            IFacingStrategy strat = new NpcFacingStrategy();
            SetFacingDirection(strat.Execute(transform.position, other.transform.position));
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            _isMoving = true;
            GetCharacterVelocity();
        }
    }
}
