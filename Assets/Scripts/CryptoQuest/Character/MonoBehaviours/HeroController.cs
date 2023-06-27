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
        [SerializeField] private Animator _animator;
        [SerializeField]
        private IInteractionManager _test;

        private Rigidbody2D _rigidbody2D;
        private Vector2 _inputVector;
        private ICharacterController2D _controller;
        private IInteractionManager _interactionManager;

        private void OnValidate()
        {
            
        }

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _controller = new TopDownController(_speed);
            _interactionManager = GetComponent<IInteractionManager>();
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
            _rigidbody2D.velocity = _controller.CalculateVelocity();
        }

        private void MoveEvent_Raised(Vector2 inputVector)
        {
            _controller.InputVector = inputVector;
        }
    }
}