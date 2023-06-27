using CryptoQuest.Characters;
using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.Character.MonoBehaviours
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class HeroController : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private float _speed = 4f;

        private Rigidbody2D _rigidbody2D;
        private Vector2 _inputVector;
        private ICharacterController2D _controller;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _controller = new TopDownController(_speed);
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
            IInteractable currentInteractable = GetComponent<InteractionManager>().CurrentInteraction;

            if (currentInteractable == null) return;
            currentInteractable.Interact();
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