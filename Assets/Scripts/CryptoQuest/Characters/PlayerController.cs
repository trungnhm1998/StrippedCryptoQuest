using CryptoQuest.Input;
using UnityEngine;


namespace CryptoQuest.Characters
{
    public class PlayerController : MonoBehaviour, ICharacter
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private PlayerStateSO _playerStateSO;

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
            _playerStateSO.facingDirection = facingDirection;
        }

        public Character.EFacingDirection GetFacingDirection()
        {
            return _playerStateSO.facingDirection;
        }
    }
}