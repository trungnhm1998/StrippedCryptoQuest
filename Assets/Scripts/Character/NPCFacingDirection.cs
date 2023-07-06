using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Character.MonoBehaviours;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest
{
    public class NPCFacingDirection : MonoBehaviour
    {
        private SpriteRenderer _currentSprite;
        [SerializeField] private List<Sprite> _listSprites;
        private string _spriteName;
        private string _spriteLocation;
        private string _assetPath;
        public CharacterBehaviour.EFacingDirection _entranceFacingDirection;

        void Start()
        {
            InitializeSetup();
            GetFacingDirectionSprites();
        }
        private void InitializeSetup()
        {
            _currentSprite = GetComponent<SpriteRenderer>();
            string spritePath = AssetDatabase.GetAssetPath(_currentSprite.sprite);
            string spriteName = _currentSprite.sprite.name;
            _assetPath = spritePath.TrimEnd('/').Remove(spritePath.LastIndexOf('/') + 1);
            _spriteName = spriteName.TrimEnd('_').Remove(spriteName.LastIndexOf("_") + 1);
            _spriteLocation = spriteName.TrimEnd('_').Remove(spriteName.LastIndexOf("_"));
        }
        public void NPCInteract()
        {
            int facingDirection;
            switch (_entranceFacingDirection)
            {
                case CharacterBehaviour.EFacingDirection.South:
                    facingDirection = (int)CharacterBehaviour.EFacingDirection.North;
                    _currentSprite.sprite = _listSprites[facingDirection];
                    Debug.Log(facingDirection);
                    break;
                case CharacterBehaviour.EFacingDirection.West:
                    facingDirection = (int)CharacterBehaviour.EFacingDirection.East;
                    _currentSprite.sprite = _listSprites[facingDirection];
                    Debug.Log(facingDirection);
                    break;
                case CharacterBehaviour.EFacingDirection.North:
                    facingDirection = (int)CharacterBehaviour.EFacingDirection.South;
                    _currentSprite.sprite = _listSprites[facingDirection];
                    Debug.Log(facingDirection);
                    break;
                case CharacterBehaviour.EFacingDirection.East:
                    facingDirection = (int)CharacterBehaviour.EFacingDirection.West;
                    _currentSprite.sprite = _listSprites[facingDirection];
                    Debug.Log(facingDirection);
                    break;
                default:
                    break;
            }
        }

        private void GetFacingDirectionSprites()
        {
            Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(_assetPath + _spriteLocation + ".png");

            for (int i = 0; i < 4; i++)
            {
                foreach (Object sprite in sprites)
                {
                    if (sprite.name == _spriteName + i)
                    {
                        _listSprites.Add(sprite as Sprite);
                    }
                }
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            NPCInteract();
        }
    }
}
