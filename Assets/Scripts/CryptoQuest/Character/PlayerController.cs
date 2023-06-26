using CryptoQuest.Characters;
using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.Character
{
    [RequireComponent(typeof(CharacterBehaviour))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;

        private CharacterBehaviour _characterBehaviour;

        private Character.EFacingDirection _facingDirection;
        private IInteractable _currentInteractable;

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

        private void MoveEvent_Raised(Vector2 axis)
        {
            Debug.Log(axis);
        }

        public void SetFacingDirection(Character.EFacingDirection facingDirection)
        {
            _facingDirection = facingDirection;
        }

        public void SaveFacingDirection(Character.EFacingDirection facingDirection)
        {
            _characterBehaviour.FacingDirection = facingDirection;
        }

        private void InteractEvent_Raised()
        {
            if (_currentInteractable == null) return;
            _currentInteractable.Interact();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(other.gameObject.name);
            _currentInteractable = other.gameObject.GetComponent<IInteractable>();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<IInteractable>() == _currentInteractable)
                _currentInteractable = null;
        }
    }
}