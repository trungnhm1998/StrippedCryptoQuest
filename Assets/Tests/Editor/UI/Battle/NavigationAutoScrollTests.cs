using CryptoQuest.UI.Battle.CommandsMenu;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Tests.Editor.UI.Battle
{
    [TestFixture]
    public class NavigationAutoScrollTests
    {
        private AutoScrollViewCalculator _autoScrollViewCalculator;
        private ScrollRect _scrollRect;
        private RectTransform _contentRect;
        private RectTransform _viewportRect;
        private RectTransform _targetRect;

        [SetUp]
        public void SetUp()
        {
            _scrollRect = new GameObject().AddComponent<ScrollRect>();
            _contentRect = new GameObject().AddComponent<RectTransform>();
            _viewportRect = new GameObject().AddComponent<RectTransform>();
            _targetRect = new GameObject().AddComponent<RectTransform>();

            _autoScrollViewCalculator = new GameObject().AddComponent<AutoScrollViewCalculator>();
        }

        [TestCase(100, 50, 100, 100, 0, TestName = "CalculateNormalizedScrollPosition_ContentLessThanViewport_ReturnsZero")]
        [TestCase(100, 200, 100, 100, 1, TestName = "CalculateNormalizedScrollPosition_ContentGreaterThanViewport_ReturnsNormalizedPosition")]
        [TestCase(0, 0, 100, 100, 0, TestName = "CalculateNormalizedScrollPosition_ZeroContentAndViewport_ReturnsZero")]
        [TestCase(100, 0, 100, 100, 0, TestName = "CalculateNormalizedScrollPosition_ZeroViewport_ReturnsZero")]
        public void CalculateNormalizedScrollPosition_ValidContentAndViewPort_ReturnsCorrectNormalizedPosition(
            float contentX,
            float contentY,
            float viewportX,
            float viewportY,
            float expectedNormalizedPosition)
        {
            _contentRect.sizeDelta = new Vector2(contentX, contentY);
            _viewportRect.sizeDelta = new Vector2(viewportX, viewportY);

            _scrollRect.content = _contentRect;
            _scrollRect.viewport = _viewportRect;

            _targetRect.sizeDelta = new Vector2(Mathf.Min(contentX, viewportX), Mathf.Min(contentY, viewportY));

            float result = _autoScrollViewCalculator.CalculateNormalizedScrollPosition(_scrollRect, _targetRect);

            Assert.AreEqual(expectedNormalizedPosition, result, 0.001f);
        }
    }
}