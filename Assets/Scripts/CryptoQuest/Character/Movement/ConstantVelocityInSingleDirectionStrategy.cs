using System;
using CryptoQuest.Character.MonoBehaviours;
using UnityEngine;

namespace CryptoQuest.Character.Movement
{
    public class ConstantVelocityInSingleDirectionStrategy : IPlayerVelocityStrategy
    {
        private Vector2 _lastMovingDirection;
        private readonly CharacterBehaviour _characterBehaviour;

        public ConstantVelocityInSingleDirectionStrategy(CharacterBehaviour characterBehaviour)
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

            // Yes I know this is not actually normalized, using Max or Min here does the same thing
            var normalizedX = inputVector.x > 0 ? 1 : inputVector.x < 0 ? -1 : 0;
            var normalizedY = inputVector.y > 0 ? 1 : inputVector.y < 0 ? -1 : 0;
            var isMovingHorizontal = Mathf.Abs(inputVector.x) >= Mathf.Abs(inputVector.y); // moving horizontal by default
            var isMovingBackward = isMovingHorizontal ? inputVector.x < 0 ? -1 : 1 : inputVector.y < 0 ? -1 : 1;
            var expectedDirection =
                isMovingHorizontal ? new Vector2(isMovingBackward, 0f) : new Vector2(0f, isMovingBackward);

            if (normalizedX != 0 && normalizedX == _lastMovingDirection.x && normalizedY == 1 && isMovingHorizontal)
                return _lastMovingDirection * speed;

            _characterBehaviour.SetFacingDirection(expectedDirection);

            _lastMovingDirection = expectedDirection;

            return expectedDirection * speed;
        }

    }
}