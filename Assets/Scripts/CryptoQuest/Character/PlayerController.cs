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

        private void Start()
        {
            _inputMediator.EnableMapGameplayInput();
        }

        private void OnEnable()
        {
            _inputMediator.MoveEvent += MoveEvent_Raised;
        }

        private void OnDisable()
        {
            _inputMediator.MoveEvent -= MoveEvent_Raised;
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
    }
}