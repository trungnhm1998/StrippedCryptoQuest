using System.Collections;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Item;
using CryptoQuest.UI.Menu.Panels.Status.Equipment;
using DG.Tweening;
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
        ITooltip WithPosition(Vector3 tooltipPositionPosition);
        ITooltip WithContentAwareness(RectTransform tooltipPosition);
        ITooltip SetSafeArea(RectTransform tooltipSafeArea);
        ITooltip WithLevel(int equipmentLevel);
        ITooltip WithRarity(RaritySO rarity);
    }

    public class UIEquipmentTooltip : MonoBehaviour, ITooltip
    {
        [SerializeField] private TooltipProvider _tooltipProvider;
        [SerializeField] private GameObject _content;

        [SerializeField] private GameObject _frame;
        [SerializeField] private GameObject _reverseFrame;
        [SerializeField] private LocalizeStringEvent _descriptionString;
        [SerializeField] private TMP_Text _effectDescription;
        [SerializeField] private Image _illustration;
        [SerializeField] private Image _rarity;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private float _waitBeforePopupTooltip;
        private bool _isShowDownWard;
        private bool _isShowTooltip;
        private Tween _tween;
        private RectTransform _contentRect;


        private void Awake()
        {
            _tooltipProvider.Tooltip = this;
            _contentRect = _content.GetComponent<RectTransform>();
        }

        /// <summary>
        /// Use this to hide or it could hide automatically if you setup
        /// <see cref="UITooltipTrigger"/> correctly
        /// </summary>
        public void Hide()
        {
            if (_handle.IsValid()) Addressables.Release(_handle);
            _handle = default;
            _tween?.Kill();
            _content.SetActive(false);
        }

        public ITooltip WithHeader(LocalizedString dataDisplayName)
        {
            return this;
        }

        public ITooltip WithDescription(LocalizedString dataDescription)
        {
            _descriptionString.StringReference = dataDescription;
            return this;
        }

        public ITooltip WithDisplaySprite(Sprite equipmentImage)
        {
            if (equipmentImage == null) return this;
            _illustration.sprite = equipmentImage;
            return this;
        }

        public ITooltip WithDisplaySprite(AssetReferenceT<Sprite> equipmentTypeIcon)
        {
            if (equipmentTypeIcon == null) return this;
            StartCoroutine(LoadSpriteAndSet(equipmentTypeIcon));
            return this;
        }

        private IEnumerator LoadSpriteAndSet(AssetReferenceT<Sprite> equipmentTypeIcon)
        {
            if (equipmentTypeIcon.RuntimeKeyIsValid() == false) yield break;
            _handle = equipmentTypeIcon.LoadAssetAsync<Sprite>();
            yield return _handle;
            _illustration.sprite = _handle.Result;
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

        private int _equipmentLevel;

        public ITooltip WithLevel(int equipmentLevel)
        {
            _equipmentLevel = equipmentLevel;
            return this;
        }

        public ITooltip WithRarity(RaritySO rarity)
        {
            _rarity.sprite = rarity.Icon;
            return this;
        }

        /// <summary>
        /// I don't know what to name this variable.
        /// It is the rect transform of the content that the tooltips will show over.
        /// Could use this as position only
        ///
        /// or use it size to 
        /// </summary>
        private RectTransform _tooltipTarget;
        private AsyncOperationHandle<Sprite> _handle;

        /// <summary>
        /// Try to place the tooltip either above or below the rect transform
        /// </summary>
        /// <param name="tooltipPosition"></param>
        /// <returns></returns>
        public ITooltip WithContentAwareness(RectTransform tooltipPosition)
        {
            _tooltipTarget = tooltipPosition;
            return this;
        }

        public void Show()
        {
            Hide();
            _tween = DOVirtual.DelayedCall(_waitBeforePopupTooltip, SetupAndShow);
        }

        private void SetupAndShow()
        {
            _level.text = $"Lv. {_equipmentLevel}";
            SetupPositionBaseOnSafeAreaAndTarget();
            _content.SetActive(true);
        }

        private void SetupPositionBaseOnSafeAreaAndTarget()
        {
            var contentSize = _contentRect.rect.size;
            var targetPosition = _tooltipTarget.position;

            _content.transform.position = targetPosition; // center by default

            if (HasSafeArea == false) return;

            var tooltipPosition = _content.transform.position;
            if (tooltipPosition.y + contentSize.y > _safeArea.position.y + _safeArea.rect.yMax)
            {
                _contentRect.pivot = new Vector2(_contentRect.pivot.x, 1);
                ToggleFrame(true);
                targetPosition = new Vector2(targetPosition.x, targetPosition.y + _tooltipTarget.rect.yMin);
            }
            else if (tooltipPosition.y - contentSize.y < _safeArea.position.y + _safeArea.rect.yMin)
            {
                _contentRect.pivot = new Vector2(_contentRect.pivot.x, 0);
                ToggleFrame(false);
                targetPosition = new Vector2(targetPosition.x, targetPosition.y + _tooltipTarget.rect.yMax);
            }

            _content.transform.position = targetPosition;
        }

        private void ToggleFrame(bool isUp)
        {
            _frame.SetActive(isUp);
            _reverseFrame.SetActive(!isUp);
        }
    }
}