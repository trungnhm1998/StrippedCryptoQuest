using CryptoQuest.Item;
using CryptoQuest.UI.Menu.Panels.Status.Equipment;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.ResourceManagement.AsyncOperations;
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
        ITooltip WithDisplaySprite(AssetReferenceT<Sprite> equipmentTypeIcon);
        ITooltip WithPosition(Vector3 tooltipPosition);
        ITooltip WithLocalPosition(Vector3 tooltipPosition);
        ITooltip WithScale(Vector3 tooltipScale);
        ITooltip WithContentAwareness(RectTransform tooltipPosition);
        ITooltip SetSafeArea(RectTransform tooltipSafeArea);
        ITooltip WithLevel(int equipmentLevel);
        ITooltip WithRarity(RaritySO rarity);
        ITooltip WithDelayTimeToDisplay(float second);
        ITooltip WithBoderPointer(bool hasPointer);
        ITooltip WithRangePivot(Vector2 minPivot, Vector2 maxPivot);
        ITooltip WithPivot(Vector2 pivot);
    }
    public class UITooltip : MonoBehaviour, ITooltip
    {
        [field:SerializeField] public ETooltipType Type { get; private set; } 
        [SerializeField] private GameObject _content;

        [SerializeField] private GameObject _nonPointerFrame;
        [SerializeField] private GameObject _pointerFrame;
        [SerializeField] private GameObject _pointerReverseFrame;
        [SerializeField] private LocalizeStringEvent _descriptionString;
        [SerializeField] private Image _illustration;
        [SerializeField] private float _waitBeforePopupTooltip;

        private Tween _tween;
        private RectTransform _contentRect;

        private RectTransform _tooltipTarget;
        private RectTransform _safeArea;
        private AsyncOperationHandle<Sprite> _handle;
        private bool HasSafeArea => _safeArea != null;
        private bool _hasPointerBoder = true;
        private Vector2 _minPivot;
        private Vector2 _maxPivot;

        private void Awake()
        {
            _contentRect = _content.GetComponent<RectTransform>();
        }

        public void Hide()
        {
            if (_handle.IsValid()) Addressables.Release(_handle);
            _handle = default;
            _tween?.Kill();
            _content.SetActive(false);
        }

        public virtual ITooltip SetSafeArea(RectTransform tooltipSafeArea)
        {
            _safeArea = tooltipSafeArea;
            return this;
        }

        public void Show()
        {
            _tween = DOVirtual.DelayedCall(_waitBeforePopupTooltip, SetupAndShow);
        }

        public virtual ITooltip WithContentAwareness(RectTransform tooltipPosition)
        {
            _tooltipTarget = tooltipPosition;
            return this;
        }

        public virtual ITooltip WithDescription(LocalizedString dataDescription)
        {
            _descriptionString.StringReference = dataDescription;
            return this;
        }

        public virtual ITooltip WithDisplaySprite(Sprite itemIcon)
        {
            if (itemIcon == null) return this;
            _illustration.sprite = itemIcon;
            return this;
        }

        public virtual ITooltip WithDisplaySprite(AssetReferenceT<Sprite> itemIcon)
        {
            if (itemIcon == null) return this;
            StartCoroutine(LoadSpriteAndSet(itemIcon));
            return this;
        }

        public virtual ITooltip WithHeader(LocalizedString dataDisplayName)
        {
            return this;
        }

        public virtual ITooltip WithLevel(int equipmentLevel)
        {
            return this;
        }

        public virtual ITooltip WithPosition(Vector3 tooltipPosition)
        {
            _content.transform.position = tooltipPosition;
            return this;
        }

        public virtual ITooltip WithLocalPosition(Vector3 tooltipPosition)
        {
            _content.transform.localPosition = tooltipPosition;
            return this;
        }

        public virtual ITooltip WithScale(Vector3 tooltipScale)
        {
            _content.transform.localScale = tooltipScale;
            return this;
        }    

        public virtual ITooltip WithRarity(RaritySO rarity)
        {
            return this;
        }

        public ITooltip WithDelayTimeToDisplay(float second)
        {
            _waitBeforePopupTooltip = second;
            return this;
        }

        public ITooltip WithBoderPointer(bool hasPointer)
        {
            _hasPointerBoder = hasPointer;
            _nonPointerFrame.SetActive(!hasPointer);
            _pointerFrame.SetActive(hasPointer);
            _pointerReverseFrame.SetActive(hasPointer);
            ToggleFrame(true);
            return this;
        }    

        public ITooltip WithRangePivot(Vector2 minPivot, Vector2 maxPivot)
        {
            _minPivot = minPivot;
            _maxPivot = maxPivot;
            return this;
        }

        public ITooltip WithPivot(Vector2 pivot)
        {
            _contentRect.pivot = pivot;
            return this;
        }    

        private void SetupAndShow()
        {
            SetupPositionBaseOnSafeAreaAndTarget();
            _content.SetActive(true);
        }

        private void SetupPositionBaseOnSafeAreaAndTarget()
        {
            var contentSize = _contentRect.rect.size;

            if (_tooltipTarget != null)
            {
                var targetPosition = _tooltipTarget.position;

                _content.transform.position = targetPosition; // center by default

                if (HasSafeArea == false) return;

                var tooltipPosition = _content.transform.position;
                if (tooltipPosition.y + contentSize.y > _safeArea.position.y + _safeArea.rect.yMax)
                {
                    _contentRect.pivot = new Vector2(_minPivot.x, _maxPivot.y);
                    ToggleFrame(true);
                    targetPosition = new Vector2(targetPosition.x, targetPosition.y + _tooltipTarget.rect.yMin);
                }
                else if (tooltipPosition.y - contentSize.y < _safeArea.position.y + _safeArea.rect.yMin)
                {
                    _contentRect.pivot = new Vector2(_minPivot.x, _minPivot.y);
                    ToggleFrame(false);
                    targetPosition = new Vector2(targetPosition.x, targetPosition.y + _tooltipTarget.rect.yMax);
                }

                _content.transform.position = targetPosition;
            }
        }

        private void ToggleFrame(bool isUp)
        {
            if(_hasPointerBoder)
            {
                _pointerFrame.SetActive(isUp);
                _pointerReverseFrame.SetActive(!isUp);
            }    
        }

        private IEnumerator LoadSpriteAndSet(AssetReferenceT<Sprite> itemIcon)
        {
            if (itemIcon.RuntimeKeyIsValid() == false) yield break;
            _handle = itemIcon.LoadAssetAsync<Sprite>();
            yield return _handle;
            _illustration.sprite = _handle.Result;
        }
    }
}
