using UnityEngine;

namespace CryptoQuest.Character.Movement
{
    public class ConstantVelocityInSingleDirectionStrategy : IPlayerVelocityStrategy
    {
        private Vector2 _lastInputVector;
        private int _lastDirection;

        public Vector2 CalculateVelocity(Vector2 inputVector, float speed)
        {
            if (speed <= 0f || inputVector == Vector2.zero)
                return Vector2.zero;

            var isMovingVertical = Mathf.Abs(inputVector.y) >= Mathf.Abs(inputVector.x); // moving vertical by default
            var direction = isMovingVertical ? inputVector.y < 0 ? -1 : 1 : inputVector.x < 0 ? -1 : 1;

            var x = inputVector.x > 0 ? 1 : inputVector.x < 0 ? -1 : 0;
            var y = inputVector.y > 0 ? 1 : inputVector.y < 0 ? -1 : 0;

            if ((Mathf.Abs(x) > 0 && x == _lastInputVector.x))
                return _lastInputVector * speed;

            inputVector = isMovingVertical ? new Vector2(0f, direction) : new Vector2(direction, 0f);

            _lastInputVector = inputVector;
            _lastDirection = direction;

            return inputVector * speed;
        }
    }
}