using CryptoQuest.Character.Movement;
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
        private IPlayerVelocityStrategy _velocityStrategy;
        private IInteractionManager _interactionManager;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _velocityStrategy = new ConstantVelocityInSingleDirectionStrategy();
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
            var velocity = _velocityStrategy.CalculateVelocity(_inputVector, _speed);

            _characterBehaviour.IsWalking = _speed > 0f && velocity != Vector2.zero;
            _characterBehaviour.SetFacingDirection(_inputVector);

            _rigidbody2D.velocity = velocity;
        }

        private void MoveEvent_Raised(Vector2 inputVector)
        {
            _inputVector = inputVector;
        }
    }
}