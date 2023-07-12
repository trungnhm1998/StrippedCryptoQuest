using System;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEngine;

namespace CryptoQuest.Character.MonoBehaviours
{
    public abstract class CharacterBehaviour : MonoBehaviour
    {
        [SerializeField] private protected Animator _animator;

        public enum EFacingDirection
        {
            South = 0,
            West = 1,
            North = 2,
            East = 3,
        }

        [SerializeField, ReadOnly] private protected EFacingDirection _facingDirection = EFacingDirection.South;
        public EFacingDirection FacingDirection => _facingDirection;

        public virtual bool IsWalking
        {
            set => _animator.SetBool(AnimIsWalking, value);
        }

        private protected readonly int AnimVelocityX = Animator.StringToHash("InputX");
        private protected readonly int AnimVelocityY = Animator.StringToHash("InputY");
        private protected readonly int AnimIsWalking = Animator.StringToHash("IsWalking");

        public void SetFacingDirection(EFacingDirection facingDirection)
        {
            _facingDirection = facingDirection;
            _animator.SetFloat(AnimVelocityX, 0);
            _animator.SetFloat(AnimVelocityY, 0);

            switch (_facingDirection)
            {
                case EFacingDirection.South:
                    _animator.SetFloat(AnimVelocityY, -1);
                    break;
                case EFacingDirection.West:
                    _animator.SetFloat(AnimVelocityX, -1);
                    break;
                case EFacingDirection.North:
                    _animator.SetFloat(AnimVelocityY, 1);
                    break;
                case EFacingDirection.East:
                    _animator.SetFloat(AnimVelocityX, 1);
                    break;
            }
        }

        public virtual void SetFacingDirection(Vector2 velocity)
        {
            if (velocity == Vector2.zero) return;

            _animator.SetFloat(AnimVelocityX, velocity.x);
            _animator.SetFloat(AnimVelocityY, velocity.y);

            switch (velocity)
            {
                case { x: > 0 }:
                    _facingDirection = EFacingDirection.East;
                    break;
                case { x: < 0 }:
                    _facingDirection = EFacingDirection.West;
                    break;
                case { y: > 0 }:
                    _facingDirection = EFacingDirection.North;
                    break;
                case { y: < 0 }:
                    _facingDirection = EFacingDirection.South;
                    break;
            }
        }
    }
}