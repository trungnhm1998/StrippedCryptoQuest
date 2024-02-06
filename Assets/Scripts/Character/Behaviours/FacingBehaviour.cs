using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Input;
using System;
using UnityEngine;

namespace CryptoQuest.Character.Behaviours
{
    public class FacingBehaviour : CharacterBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private Rigidbody2D _rigidbody2D;

        public EFacingDirection FacingDirection => _facingDirection;

        public event Action<EFacingDirection> OnFacingDirectionChanged;
        private Vector2 _inputVector;

        private void OnEnable()
        {
            _inputMediator.MoveEvent += MoveEvent_Raised;
            SetFacingDirection(_facingDirection);
        }

        private void OnDisable()
        {
            _inputMediator.MoveEvent -= MoveEvent_Raised;
        }

        private void MoveEvent_Raised(Vector2 inputVector)
        {
            _inputVector = inputVector;
        }

        private void Update()
        {
            IsWalking = _rigidbody2D.velocity != Vector2.zero;
            if (_inputVector != Vector2.zero)
                SetFacingDirection(_inputVector);
        }

        public override void SetFacingDirection(EFacingDirection facingDirection)
        {
            base.SetFacingDirection(facingDirection);
            OnFacingDirectionChanged?.Invoke(_facingDirection);
        }

        public override void SetFacingDirection(Vector2 velocity)
        {
            base.SetFacingDirection(velocity);
            OnFacingDirectionChanged?.Invoke(_facingDirection);
        }
    }
}
