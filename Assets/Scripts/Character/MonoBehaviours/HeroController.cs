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
        [SerializeField] private float _speed = 4f;
        [SerializeField] private CharacterBehaviour _characterBehaviour;

        private Rigidbody2D _rigidBody2D;
        private Vector2 _inputVector;
        private IPlayerVelocityStrategy _velocityStrategy;
        private IInteractionManager _interactionManager;

        private void Awake()
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _velocityStrategy = new ConstantStraightMovementStrategy(_characterBehaviour);
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

        private void Update()
        {
            _characterBehaviour.IsWalking = _speed > 0f && _rigidBody2D.velocity != Vector2.zero;
        }

        private void FixedUpdate()
        {
            _rigidBody2D.velocity = _velocityStrategy.CalculateVelocity(_inputVector, _speed);
        }

        private void MoveEvent_Raised(Vector2 inputVector)
        {
            _inputVector = inputVector;
        }
        
        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 200, 20), $"Input Vector: {_inputVector}");
        }
    }
}