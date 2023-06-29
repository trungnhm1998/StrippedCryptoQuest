using CryptoQuest.Character.MonoBehaviours;
using UnityEngine;

namespace CryptoQuest.Character.Movement
{
    public class ConstantVelocityInSingleDirectionStrategy : IPlayerVelocityStrategy
    {
        private Vector2 _lastInputVector;

        private readonly CharacterBehaviour _characterBehaviour;

        public ConstantVelocityInSingleDirectionStrategy(CharacterBehaviour characterBehaviour)
        {
            _characterBehaviour = characterBehaviour;
        }

        public Vector2 CalculateVelocity(Vector2 inputVector, float speed)
        {
            if (speed <= 0f || inputVector == Vector2.zero)
            {
                _lastInputVector = Vector2.zero;
                return Vector2.zero;
            }

            var isMovingVertical = Mathf.Abs(inputVector.y) >= Mathf.Abs(inputVector.x); // moving vertical by default
            var direction = isMovingVertical ? inputVector.y < 0 ? -1 : 1 : inputVector.x < 0 ? -1 : 1;

            var x = inputVector.x > 0 ? 1 : inputVector.x < 0 ? -1 : 0;
            var y = inputVector.y > 0 ? 1 : inputVector.y < 0 ? -1 : 0;

            if ((!isMovingVertical && x != 0 && x == _lastInputVector.x) ||
                (isMovingVertical && y != 0 && y == _lastInputVector.y))
            {
                return _lastInputVector * speed;
            }

            inputVector = isMovingVertical ? new Vector2(0f, direction) : new Vector2(direction, 0f);

            _characterBehaviour.SetFacingDirection(inputVector);

            _lastInputVector = inputVector;

            return inputVector * speed;
        }
    }
}