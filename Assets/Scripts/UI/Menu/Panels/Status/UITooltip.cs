using CryptoQuest.UI.Menu.Panels.Status.Equipment;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Status
{
    public interface ITooltip
    {
        void Show();
        void Hide();
        ITooltip WithHeader(LocalizedString dataDisplayName);
        ITooltip WithDescription(LocalizedString dataDescription);
        ITooltip WithDisplaySprite(Sprite equipmentTypeIcon);
        ITooltip WithPosition(Vector3 tooltipPositionPosition);
        ITooltip WithContentAwareness(RectTransform tooltipPosition);
        ITooltip SetSafeArea(RectTransform tooltipSafeArea);
    }

    public class UITooltip : MonoBehaviour, ITooltip
    {
        [SerializeField] private TooltipProvider _tooltipProvider;
        [SerializeField] private GameObject _content;

        [SerializeField] private RectTransform _upContainer;
        [SerializeField] private RectTransform _downContainer;
        [SerializeField] private GameObject _frame;
        [SerializeField] private GameObject _reverseFrame;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _effectDescription;
        [SerializeField] private Image _illustration;
        [SerializeField] private Image _rarity;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private float _waitBeforePopupTooltip;
        private bool _isShowDownWard;
        private bool _isShowTooltip;
        private Tween _tween;

        private void Awake()
        {
            _tooltipProvider.Tooltip = this;
        }

        public void Hide()
        {
            _tween?.Kill();
            _content.SetActive(false);
        }

        public ITooltip WithHeader(LocalizedString dataDisplayName)
        {
            return this;
        }

        public ITooltip WithDescription(LocalizedString dataDescription)
        {
            _description.text = dataDescription.GetLocalizedString();
            return this;
        }

        public ITooltip WithDisplaySprite(Sprite equipmentImage)
        {
            if (equipmentImage == null) return this;
            _illustration.sprite = equipmentImage;
            return this;
        }

        public ITooltip WithPosition(Vector3 position)
        {
            _content.transform.position = position;
            return this;
        }

        private RectTransform _safeArea;
        private bool HasSafeArea => _safeArea != null;

        /// <summary>
        /// The tooltip will try to stay inside the safe area
        /// </summary>
        /// <param name="tooltipSafeArea"></param>
        /// <returns></returns>
        public ITooltip SetSafeArea(RectTransform tooltipSafeArea)
        {
            _safeArea = tooltipSafeArea;
            return this;
        }

        /// <summary>
        /// I don't know what to name this variable.
        /// It is the rect transform of the content that the tooltips will show over.
        /// Could use this as position only
        ///
        /// or use it size to 
        /// </summary>
        private RectTransform _tooltipContent;

        /// <summary>
        /// Try to place the tooltip either above or below the rect transform
        /// </summary>
        /// <param name="tooltipPosition"></param>
        /// <returns></returns>
        public ITooltip WithContentAwareness(RectTransform tooltipPosition)
        {
            _tooltipContent = tooltipPosition;
            return this;
        }

        public void Show()
        {
            _tween?.Kill();
            Hide();
            _tween = DOVirtual.DelayedCall(_waitBeforePopupTooltip, SetupAndShow);
        }

        private void SetupAndShow()
        {
            var rectTransform = _content.GetComponent<RectTransform>();
            var contentSize = rectTransform.rect.size;
            var targetPosition = _tooltipContent.position;

            _content.transform.position = targetPosition; // center by default

            if (HasSafeArea == false)
            {
                _content.SetActive(true);
                return;
            }

            var tooltipPosition = _content.transform.position;
            if (tooltipPosition.y + contentSize.y > _safeArea.position.y + _safeArea.rect.yMax)
            {
                rectTransform.pivot = new Vector2(rectTransform.pivot.x, 1);
                _frame.SetActive(true);
                _reverseFrame.SetActive(false);
                targetPosition = new Vector2(targetPosition.x, targetPosition.y + _tooltipContent.rect.yMin);
            }
            else if (tooltipPosition.y - contentSize.y < _safeArea.position.y + _safeArea.rect.yMin)
            {
                rectTransform.pivot = new Vector2(rectTransform.pivot.x, 0);
                _frame.SetActive(false);
                _reverseFrame.SetActive(true);
                targetPosition = new Vector2(targetPosition.x, targetPosition.y + _tooltipContent.rect.yMax);
            }

            _content.transform.position = targetPosition;
            _content.SetActive(true);
        }
    }
}