using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.Characters
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;

        private IInteractable _currentNpc;

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

        private void InteractEvent_Raised()
        {
            if (_currentNpc == null) return;
            _currentNpc.Interact();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            _currentNpc = other.gameObject.GetComponent<IInteractable>();
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            _currentNpc = null;
        }
    }
}