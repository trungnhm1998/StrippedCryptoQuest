using UnityEngine;

namespace CryptoQuest.Character
{
    public class SingleDirectionTopDownController2D : ICharacterController2D
    {
        public float Speed { get; set; }

        public Vector2 InputVector
        {
            get => _inputVector;
            set => _inputVector = value;
        }

        private Vector2 _inputVector = Vector2.zero;

        public Vector2 CalculateVelocity()
        {
            if (Speed <= 0f || InputVector == Vector2.zero)
                return Vector2.zero;

            var isMovingVertical = Mathf.Abs(InputVector.y) >= Mathf.Abs(InputVector.x); // moving vertical by default
            var direction = isMovingVertical ? _inputVector.y < 0 ? -1 : 1 : _inputVector.x < 0 ? -1 : 1;

            _inputVector = isMovingVertical ? new Vector2(0f, direction) : new Vector2(direction, 0f);

            return _inputVector * Speed;
        }
    }
}