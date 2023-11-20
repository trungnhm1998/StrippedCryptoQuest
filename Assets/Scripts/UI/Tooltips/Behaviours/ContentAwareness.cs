using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Tooltips.Behaviours
{
    public class ContentAwareness : TooltipBehaviourBase
    {
        [SerializeField] private GameObject _upArrow;
        [SerializeField] private GameObject _downArrow;

        public override void Setup()
        {
            var selectedGameObject = EventSystem.current.currentSelectedGameObject;
            if (selectedGameObject == null) return;
            var targetRectTransform = selectedGameObject.GetComponent<RectTransform>();

            var offsetY = targetRectTransform.rect.height / 2;
            var isPointingDown = RectTransform.pivot.y == 0;
            _upArrow.SetActive(!isPointingDown);
            _downArrow.SetActive(isPointingDown);
            offsetY *= isPointingDown ? 1 : -1;

            var position = RectTransform.position;
            position.y += offsetY;
            RectTransform.position = position;
        }
    }
}