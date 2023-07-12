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
        [SerializeField] private Transform _pointAPosition;
        [SerializeField] private Transform _pointBPosition;
        [SerializeField] private Transform _destination;
        [SerializeField] private float _speed;
        [SerializeField] private float _delayTime;
        private float _timeElapsed = 0;
        private bool _isMoving;
        private Vector2 _characterVelocity;

        void Start()
        {
            _isMoving = IsWalking = true;
            if (_destination == null){} _destination = _pointAPosition;
            transform.position = _destination.position;
            _animator.runtimeAnimatorController = _animatorOverrideController;
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
                IsWalking = false;
                _timeElapsed += Time.deltaTime;
                if (_timeElapsed > _delayTime)
                {
                    SwapDestination();
                    _timeElapsed = 0;
                }
            }
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
            IsWalking = true;
            GetCharacterVelocity();
        }

        private void GetCharacterVelocity()
        {
            _characterVelocity = _destination.localPosition - gameObject.transform.localPosition;
            SetFacingDirection(_characterVelocity.normalized);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _isMoving = IsWalking = false;
                IFacingStrategy strat = new NpcFacingStrategy();
                SetFacingDirection(strat.Execute(transform.position, other.transform.position));
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            _isMoving = IsWalking = true;
            GetCharacterVelocity();
        }
    }
}
