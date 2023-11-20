using System.Collections;
using System.Collections.Generic;
using CryptoQuest.UI.Menu;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.ChangeClass.View
{
    public class UIChangeClassTooltip : MonoBehaviour
    {
        [SerializeField] private RectTransform _contentRect;
        [SerializeField] private RectTransform _safeArea;

        public void ShowTooltip(UICharacter character)
        {
            RectTransform targetPosition = character.Content;
            Vector2 newPosition = new();

            var contentHeight = _contentRect.rect.height;

            if (targetPosition.position.y > _safeArea.position.y + _safeArea.rect.yMin)
            {
                newPosition = new Vector2(_contentRect.position.x, targetPosition.position.y - (contentHeight / 2) - targetPosition.rect.height);
            }
            else
            {
                newPosition = new Vector2(_contentRect.position.x, targetPosition.position.y + (contentHeight / 2) + targetPosition.rect.height);
            }
            _contentRect.transform.position = newPosition;
        }
    }
}
