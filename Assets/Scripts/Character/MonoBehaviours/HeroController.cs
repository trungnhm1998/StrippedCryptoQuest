using System;
using CryptoQuest.Character.Movement;
using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.Character.MonoBehaviours
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class HeroController : MonoBehaviour, ICharacterController
    {
        [SerializeField] private InputMediatorSO _inputMediator;

        [Tooltip("how far the character has to move to trigger a step")]
        [SerializeField] private float _distanceTilCountAsStep = .95f;

        [SerializeField] private float _speed = 4f;
        [SerializeField] private HeroBehaviour _characterBehaviour;

        private Rigidbody2D _rigidBody2D;
        private Vector2 _inputVector;
        private IPlayerVelocityStrategy _velocityStrategy;
        private IInteractionManager _interactionManager;
        
        public void SetDistanceTilNextStep(float distance)
        {
            _distanceTilCountAsStep = distance;
        }

        private void Awake()
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _velocityStrategy = new ConstantVelocityLastInputMovementStrategy();
            _interactionManager = GetComponent<IInteractionManager>();
        }

        private void OnEnable()
        {
            _inputMediator.MoveEvent += MoveEvent_Raised;
            _inputMediator.InteractEvent += InteractEvent_Raised;
        }

        private void OnDisable()
        {
            _inputMediator.MoveEvent -= MoveEvent_Raised;
            _inputMediator.InteractEvent -= InteractEvent_Raised;
        }

        private void InteractEvent_Raised()
        {
            _interactionManager.Interact();
        }

        private void Update()
        {
            _characterBehaviour.IsWalking = _speed > 0f && _rigidBody2D.velocity != Vector2.zero;
            if (_inputVector != Vector2.zero)
                _characterBehaviour.SetFacingDirection(_inputVector);
        }

        private Vector2 _lastPosition;
        private float _distance;

        private void FixedUpdate()
        {
            if (_rigidBody2D == null || _velocityStrategy == null) return;
            _rigidBody2D.velocity = _velocityStrategy.CalculateVelocity(_inputVector, _speed);
            OnCharacterStep();
        }

        /// <summary>
        /// find the magnitude of the vector (the distance) between the last position and the new position
        /// </summary>
        private void OnCharacterStep()
        {
            var position = transform.position;
            _distance +=
                Math.Abs(Vector2.Distance(_lastPosition, position)); // don't care about if moving backward or not
            _lastPosition = position;

            if (!(_distance >= _distanceTilCountAsStep)) return;
            _distance = 0;
            _characterBehaviour.OnStep();
        }

        private void MoveEvent_Raised(Vector2 inputVector)
        {
            _inputVector = inputVector;
        }
    }
}