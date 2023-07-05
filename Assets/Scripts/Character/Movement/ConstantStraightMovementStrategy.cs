using System;
using CryptoQuest.Character.MonoBehaviours;
using UnityEngine;

namespace CryptoQuest.Character.Movement
{
    public class ConstantStraightMovementStrategy : IPlayerVelocityStrategy
    {
        private Vector2 _lastMovingDirection;
        private readonly CharacterBehaviour _characterBehaviour;

        public ConstantStraightMovementStrategy(CharacterBehaviour characterBehaviour)
        {
            _characterBehaviour = characterBehaviour;
        }

        public Vector2 CalculateVelocity(Vector2 inputVector, float speed)
        {
            if (speed <= 0f || inputVector == Vector2.zero)
            {
                _lastMovingDirection = Vector2.zero;
                return Vector2.zero;
            }
            var isMovingHorizontal = Mathf.Abs(inputVector.x) >= Mathf.Abs(inputVector.y); // moving horizontal by defaultDF
            var straightMoveInput = isMovingHorizontal ? new Vector2(inputVector.x, 0) : new Vector2(0, inputVector.y);
            var expectedDirection = straightMoveInput.normalized;

            var isMovingDiagonal = inputVector.x != 0 && inputVector.y != 0; 
            if (isMovingDiagonal && _lastMovingDirection != Vector2.zero) // prevent change input when moving diagonal
                return _lastMovingDirection * speed;

            _characterBehaviour.SetFacingDirection(expectedDirection);

            _lastMovingDirection = expectedDirection;

            return expectedDirection * speed;
        }

    }
}