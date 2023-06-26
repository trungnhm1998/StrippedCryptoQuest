using CryptoQuest.Input;
using UnityEngine;


namespace CryptoQuest.Characters
{
    public class PlayerController : MonoBehaviour, ICharacter
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private CharacterStateSO _characterStateSO;
        private Character.EFacingDirection _facingDirection;

        public CharacterStateSO CharacterStateSO
        {
            get => _characterStateSO;
            set => _characterStateSO = value;
        }

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

        public Character.EFacingDirection GetFacingDirection()
        {
            return _facingDirection;
        }
        public void SaveFacingDirection(Character.EFacingDirection facingDirection)
        {
            _characterStateSO.facingDirection = facingDirection;
        }
    }
}