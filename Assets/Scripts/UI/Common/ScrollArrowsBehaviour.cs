using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.UI.Common
{
    public class ScrollArrowsBehaviour : MonoBehaviour
    {
        public static Action NavigateEvent;
        
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private RectTransform _referenceItemRect;
        
        [SerializeField] private GameObject _upArrow;
        [SerializeField] private GameObject _downArrow;

        [Tooltip("Enable this flag to run the behaviour in the Update()")]
        [SerializeField] private bool _isUseUpdate;

        private RectTransform _viewport;
        private RectTransform _content;
        private float _itemHeight;

        private void Awake()
        {
            _itemHeight = Mathf.Round(_referenceItemRect.rect.height);
            _viewport = _scrollRect.viewport;
            _content = _scrollRect.content;
        }

        private void OnEnable()
        {
            NavigateEvent += DisplayNavigateArrows;
        }

        private void OnDisable()
        {
            _upArrow.SetActive(false);
            _downArrow.SetActive(false);
            NavigateEvent -= DisplayNavigateArrows;
        }

        private bool ShouldMoveUp => Mathf.Round(_content.anchoredPosition.y) >= _itemHeight;

        private bool ShouldMoveDown => _content.rect.height - Mathf.Round(_content.anchoredPosition.y)
            > _scrollRect.viewport.rect.height + _itemHeight / 2;

        private void DisplayNavigateArrows()
        {
            _upArrow.SetActive(ShouldMoveUp);
            _downArrow.SetActive(ShouldMoveDown);
        }

        private void Update()
        {
            if (_isUseUpdate) DisplayNavigateArrows();
        }
    }
}