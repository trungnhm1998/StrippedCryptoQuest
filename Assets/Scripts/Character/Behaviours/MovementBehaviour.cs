using UnityEngine;
using CryptoQuest.Character.Movement;
using CryptoQuest.Input;

namespace CryptoQuest.Character.Behaviours
{
    public class MovementBehaviour : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;

        [Tooltip("how far the character has to move to trigger a step")]

        [SerializeField] private float _speed = 4f;

        private Vector2 _inputVector;
        private IPlayerVelocityStrategy _velocityStrategy;
        private Rigidbody2D _rigidBody2D;

        private void Awake()
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _velocityStrategy = new ConstantVelocityLastInputMovementStrategy();
        }

        private void OnEnable()
        {
            _inputMediator.MoveEvent += MoveEvent_Raised;
        }

        private void FixedUpdate()
        {
            if (_rigidBody2D == null || _velocityStrategy == null) return;
            _rigidBody2D.velocity = _velocityStrategy.CalculateVelocity(_inputVector, _speed);
        }

        private void OnDisable()
        {
            _inputMediator.MoveEvent -= MoveEvent_Raised;
        }

        private void MoveEvent_Raised(Vector2 inputVector)
        {
            _inputVector = inputVector;
        }

        public void StopMovement()
        {
            _inputVector = Vector2.zero;
        }
    }
}
