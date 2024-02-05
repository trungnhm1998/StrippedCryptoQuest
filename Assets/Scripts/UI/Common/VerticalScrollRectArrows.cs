using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Common
{
    [AddComponentMenu("CryptoQuest/UI/Common/VerticalScrollRectArrows")]
    public class VerticalScrollRectArrows : MonoBehaviour
    {
        [SerializeField] private GameObject _upArrow;
        [SerializeField] private GameObject _downArrow;

        private ScrollRect _scrollRect;
        private RectTransform _rectTransform;
        private RectTransform _contentRectTransform;

        private void Awake()
        {
            _scrollRect = GetComponent<ScrollRect>();
            _rectTransform = GetComponent<RectTransform>();
            _contentRectTransform = _scrollRect.content;
        }

        private void Update()
        {
            DisplayNavigateArrows();
        }

        private void DisplayNavigateArrows()
        {
            _upArrow.SetActive(CanScrollUp());
            _downArrow.SetActive(CanScrollDown());
        }

        private const float EPSILON = 0.001f;

        private bool CanScrollUp()
        {
            return _contentRectTransform.rect.height > _rectTransform.rect.height &&
                   _contentRectTransform.anchoredPosition.y > EPSILON;
        }

        private bool CanScrollDown()
        {
            var rect = _rectTransform.rect;
            return _contentRectTransform.rect.height > rect.height &&
                   _contentRectTransform.anchoredPosition.y <
                   _contentRectTransform.rect.height - rect.height;
        }
    }
}