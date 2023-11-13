using System.Collections;
using CryptoQuest.Item;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu
{
    public class UITooltip : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _descriptionString;
        [SerializeField] private Image _illustration;
        [SerializeField] private float _waitBeforePopupTooltip;

        private Tween _tween;
        private RectTransform _contentRect;

        private RectTransform _tooltipTarget;
        private RectTransform _safeArea;
        private AsyncOperationHandle<Sprite> _handle;
        private bool HasSafeArea => _safeArea != null;
        private bool _hasPointerBorder = true;
        private Vector2 _minPivot;
        private Vector2 _maxPivot;

        public void Hide()
        {
            if (_handle.IsValid()) Addressables.Release(_handle);
            _handle = default;
            _tween?.Kill();
            gameObject.SetActive(false);
        }

        public virtual UITooltip SetSafeArea(RectTransform tooltipSafeArea)
        {
            _safeArea = tooltipSafeArea;
            return this;
        }

        public void Show()
        {
            _tween = DOVirtual.DelayedCall(_waitBeforePopupTooltip, SetupAndShow);
        }

        public virtual UITooltip WithContentAwareness(RectTransform tooltipPosition)
        {
            _tooltipTarget = tooltipPosition;
            return this;
        }

        public virtual UITooltip WithDescription(LocalizedString dataDescription)
        {
            _descriptionString.StringReference = dataDescription;
            return this;
        }

        public virtual UITooltip WithDisplaySprite(Sprite itemIcon)
        {
            if (itemIcon == null) return this;
            _illustration.sprite = itemIcon;
            return this;
        }

        public virtual UITooltip WithDisplaySprite(AssetReferenceT<Sprite> itemIcon)
        {
            if (itemIcon == null) return this;
            StartCoroutine(LoadSpriteAndSet(itemIcon));
            return this;
        }

        public virtual UITooltip WithHeader(LocalizedString dataDisplayName)
        {
            return this;
        }

        public virtual UITooltip WithLevel(int equipmentLevel)
        {
            return this;
        }

        public virtual UITooltip WithPosition(Vector3 tooltipPosition)
        {
            // _content.transform.position = tooltipPosition;
            return this;
        }

        public virtual UITooltip WithLocalPosition(Vector3 tooltipPosition)
        {
            // _content.transform.localPosition = tooltipPosition;
            return this;
        }

        public virtual UITooltip WithScale(Vector3 tooltipScale)
        {
            // _content.transform.localScale = tooltipScale;
            return this;
        }

        public virtual UITooltip WithRarity(RaritySO rarity)
        {
            return this;
        }

        public UITooltip WithDelayTimeToDisplay(float second)
        {
            _waitBeforePopupTooltip = second;
            return this;
        }

        public UITooltip WithBorderPointer(bool hasPointer)
        {
            _hasPointerBorder = hasPointer;
            // _nonPointerFrame.SetActive(!hasPointer);
            // _pointerFrame.SetActive(hasPointer);
            // _pointerReverseFrame.SetActive(hasPointer);
            ToggleFrame(true);
            return this;
        }

        public UITooltip WithRangePivot(Vector2 minPivot, Vector2 maxPivot)
        {
            _minPivot = minPivot;
            _maxPivot = maxPivot;
            return this;
        }

        public UITooltip WithPivot(Vector2 pivot)
        {
            _contentRect.pivot = pivot;
            return this;
        }    

        private void SetupAndShow()
        {
            SetupPositionBaseOnSafeAreaAndTarget();
            gameObject.SetActive(true);
        }

        private void SetupPositionBaseOnSafeAreaAndTarget()
        {
            // var contentSize = _contentRect.rect.size;
            //
            // if (_tooltipTarget != null)
            // {
            //     var targetPosition = _tooltipTarget.position;
            //
            //     _content.transform.position = targetPosition; // center by default
            //
            //     if (HasSafeArea == false) return;
            //
            //     var tooltipPosition = _content.transform.position;
            //     if (tooltipPosition.y + contentSize.y > _safeArea.position.y + _safeArea.rect.yMax)
            //     {
            //         _contentRect.pivot = new Vector2(_minPivot.x, _maxPivot.y);
            //         ToggleFrame(true);
            //         targetPosition = new Vector2(targetPosition.x, targetPosition.y + _tooltipTarget.rect.yMin);
            //     }
            //     else if (tooltipPosition.y - contentSize.y < _safeArea.position.y + _safeArea.rect.yMin)
            //     {
            //         _contentRect.pivot = new Vector2(_minPivot.x, _minPivot.y);
            //         ToggleFrame(false);
            //         targetPosition = new Vector2(targetPosition.x, targetPosition.y + _tooltipTarget.rect.yMax);
            //     }
            //
            //     _content.transform.position = targetPosition;
            // }
        }

        private void ToggleFrame(bool isUp)
        {
            // if (!_hasPointerBorder) return;
            // _pointerFrame.SetActive(isUp);
            // _pointerReverseFrame.SetActive(!isUp);
        }

        private IEnumerator LoadSpriteAndSet(AssetReferenceT<Sprite> itemIcon)
        {
            if(!itemIcon.OperationHandle.IsValid())
            {
                if (itemIcon.RuntimeKeyIsValid() == false) yield break;
                _handle = itemIcon.LoadAssetAsync<Sprite>();
                yield return _handle;
            }
            _illustration.sprite = (Sprite)itemIcon.OperationHandle.Result;
        }
    }
}