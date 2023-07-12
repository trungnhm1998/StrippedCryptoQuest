using CryptoQuest.Character.MonoBehaviours;
using UnityEngine;

namespace CryptoQuest.Character
{
    public class NPCFacingDirection : MonoBehaviour
    {
        [SerializeField] private CharacterBehaviour.EFacingDirection _characterFacingDirection;
        [SerializeField] private HeroBehaviour _heroFacingDirection;
        [SerializeField] private SpriteRenderer _currentSprite;
        [SerializeField] private Sprite _FacingDownDirection;
        [SerializeField] private Sprite _FacingLeftDirection;
        [SerializeField] private Sprite _FacingTopDirection;
        [SerializeField] private Sprite _FacingRightDirection;

        void Start()
        {
            _currentSprite = GetComponent<SpriteRenderer>();
        }
        public void NPCInteract()
        {
            _characterFacingDirection = _heroFacingDirection.FacingDirection;
            switch (_characterFacingDirection)
            {
                case CharacterBehaviour.EFacingDirection.South:
                    _currentSprite.sprite = _FacingTopDirection;
                    break;
                case CharacterBehaviour.EFacingDirection.West:
                    _currentSprite.sprite = _FacingRightDirection;
                    break;
                case CharacterBehaviour.EFacingDirection.North:
                    _currentSprite.sprite = _FacingDownDirection;
                    break;
                case CharacterBehaviour.EFacingDirection.East:
                    _currentSprite.sprite = _FacingLeftDirection;
                    break;
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("InteractionZone") && _heroFacingDirection == null)
            {
                _heroFacingDirection = other.GetComponentInParent<HeroBehaviour>();
            }
        }
    }
}
