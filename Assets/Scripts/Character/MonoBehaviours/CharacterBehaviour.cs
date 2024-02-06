using IndiGames.Core.Common;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEngine;

namespace CryptoQuest.Character.MonoBehaviours
{
    public abstract class CharacterBehaviour : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        protected Animator _animatorComponent
        {
            get
            {
                if (_animator == null)
                    _animator = GetComponent<Animator>();
                return _animator;
            }
        }

        public enum EFacingDirection
        {
            South = 0,
            West = 1,
            North = 2,
            East = 3,
        }

        [SerializeField, ReadOnly] protected EFacingDirection _facingDirection = EFacingDirection.South;

        public virtual bool IsWalking
        {
            set => _animatorComponent.SetBool(AnimIsWalking, value);
        }

        private static readonly int AnimVelocityX = Animator.StringToHash("InputX");
        private static readonly int AnimVelocityY = Animator.StringToHash("InputY");
        private static readonly int AnimIsWalking = Animator.StringToHash("IsWalking");
        private static readonly int AnimFacingDirection = Animator.StringToHash("IsFacingToHero");

        public virtual void SetFacingDirection(EFacingDirection facingDirection)
        {
            _facingDirection = facingDirection;
            _animatorComponent.SetBool(AnimFacingDirection, true);
            _animatorComponent.SetFloat(AnimVelocityX, 0);
            _animatorComponent.SetFloat(AnimVelocityY, 0);

            switch (_facingDirection)
            {
                case EFacingDirection.South:
                    _animatorComponent.SetFloat(AnimVelocityY, -1);
                    break;
                case EFacingDirection.West:
                    _animatorComponent.SetFloat(AnimVelocityX, -1);
                    break;
                case EFacingDirection.North:
                    _animatorComponent.SetFloat(AnimVelocityY, 1);
                    break;
                case EFacingDirection.East:
                    _animatorComponent.SetFloat(AnimVelocityX, 1);
                    break;
            }
        }

        public virtual void SetFacingDirection(Vector2 velocity)
        {
            if (velocity == Vector2.zero) return;
            velocity = velocity.normalized;

            _animatorComponent.SetFloat(AnimVelocityX, velocity.x);
            _animatorComponent.SetFloat(AnimVelocityY, velocity.y);

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