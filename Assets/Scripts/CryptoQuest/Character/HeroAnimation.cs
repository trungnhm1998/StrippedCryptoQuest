using CryptoQuest.Character.MonoBehaviours;
using UnityEngine;

namespace CryptoQuest.Character
{
    public class HeroAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _heroAnimation;
        private CharacterBehaviour.EFacingDirection _facingDirection;
        private Rigidbody2D _rigidbody2D;

        void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            SetAnimation();
        }

        private void SetAnimation()
        {
            if (_rigidbody2D.velocity.x != 0 || _rigidbody2D.velocity.y != 0)
            {
                _heroAnimation.SetFloat("moveX", _rigidbody2D.velocity.x);
                _heroAnimation.SetFloat("moveY", _rigidbody2D.velocity.y);
            }
            else
            {
                _heroAnimation.SetFloat("moveX", 0);
                _heroAnimation.SetFloat("moveY", 0);
            }

            SetFacingDirection();
        }

        private void SetFacingDirection()
        {
            switch (_rigidbody2D.velocity.x, _rigidbody2D.velocity.y)
            {
                case (0, > 0):
                    _facingDirection = CharacterBehaviour.EFacingDirection.North;
                    _heroAnimation.SetFloat("lastPosX", 0);
                    _heroAnimation.SetFloat("lastPosY", .1f);
                    break;
                case (0, < 0):
                    _facingDirection = CharacterBehaviour.EFacingDirection.South;
                    _heroAnimation.SetFloat("lastPosX", 0);
                    _heroAnimation.SetFloat("lastPosY", -.1f);
                    break;
                case (< 0, 0):
                    _facingDirection = CharacterBehaviour.EFacingDirection.West;
                    _heroAnimation.SetFloat("lastPosX", -.1f);
                    _heroAnimation.SetFloat("lastPosY", 0);
                    break;
                case (> 0, 0):
                    _facingDirection = CharacterBehaviour.EFacingDirection.East;
                    _heroAnimation.SetFloat("lastPosX", .1f);
                    _heroAnimation.SetFloat("lastPosY", 0);
                    break;
                default:
                    break;
            }
        }
    }
}