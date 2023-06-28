using System;
using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.Character.MonoBehaviours
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class HeroController : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private float _speed = 4f;
        [SerializeField] private CharacterBehaviour _characterBehaviour;

        private Rigidbody2D _rigidbody2D;
        private Vector2 _inputVector;
        private ICharacterController2D _controller;
        private IInteractionManager _interactionManager;

        private static readonly int MoveX = Animator.StringToHash("moveX");

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _controller = new SingleDirectionTopDownController2D();
            _interactionManager = GetComponent<IInteractionManager>();
            // _characterBehaviour = GetComponent<CharacterBehaviour>();
        }

        private void Start()
        {
            _inputMediator.EnableMapGameplayInput();
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

        private void FixedUpdate()
        {
            _controller.Speed = _speed;
            var velocity = _controller.CalculateVelocity();

            _characterBehaviour.IsWalking = _speed > 0f && velocity != Vector2.zero;
            _characterBehaviour.SetFacingDirection(velocity);

            _rigidbody2D.velocity = velocity;
        }

        private Vector2 _i;

        private void MoveEvent_Raised(Vector2 inputVector)
        {
            _i = inputVector;
            _controller.InputVector = inputVector;
        }

        // render debug info
        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 300, 20), $"InputVector: {_controller.InputVector}");
            GUI.Label(new Rect(10, 30, 300, 20), $"Velocity: {_controller.CalculateVelocity()}");
            GUI.Label(new Rect(10, 50, 300, 20), $"i {_i}");
        }
    }
}