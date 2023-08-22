using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace CryptoQuest.Character.MonoBehaviours
{
    public class CharacterSortingOrder : MonoBehaviour
    {
        [SerializeField, ReadOnly] private string _npcTag = "NPC";
        [SerializeField] private SpriteRenderer _playerSpriteRenderer;
        private int _cachedNpcOrder;
        private SpriteRenderer _npcSpriteRenderer;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(_npcTag))
            {
                _npcSpriteRenderer = other.GetComponentInParent<SpriteRenderer>();
                _cachedNpcOrder = _npcSpriteRenderer.sortingOrder;
                SortingLayer();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(_npcTag))
                _npcSpriteRenderer.sortingOrder = _cachedNpcOrder;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag(_npcTag))
                SortingLayer();
        }

        private void SortingLayer()
        {
            if (_playerSpriteRenderer.transform.position.y > _npcSpriteRenderer.transform.position.y)
                _npcSpriteRenderer.sortingOrder = _playerSpriteRenderer.sortingOrder + 1;
            else
                _npcSpriteRenderer.sortingOrder = _playerSpriteRenderer.sortingOrder - 1;
        }
    }
}
