using System;
using Core.Runtime.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEngine;

namespace CryptoQuest.Character.MonoBehaviours
{
    public abstract class CharacterBehaviour : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public enum EFacingDirection
        {
            South = 0,
            West = 1,
            North = 2,
            East = 3,
        }

        [SerializeField, ReadOnly] private EFacingDirection _facingDirection = EFacingDirection.South;

        public bool IsWalking
        {
            set => _animator.SetBool(AnimIsWalking, value);
        }

        private static readonly int AnimVelocityX = Animator.StringToHash("InputX");
        private static readonly int AnimVelocityY = Animator.StringToHash("InputY");
        private static readonly int AnimIsWalking = Animator.StringToHash("IsWalking");

        public void SetFacingDirection(Vector2 velocity)
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