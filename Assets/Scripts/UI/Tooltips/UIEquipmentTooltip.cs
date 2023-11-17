using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Extensions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace CryptoQuest.UI.Tooltips
{
    public class UIEquipmentTooltip : MonoBehaviour
    {
        [SerializeField] private Image _headerBackground;
        [SerializeField] private Image _rarity;
        [SerializeField] private GameObject _nftTag;
        [SerializeField] private Image _illustration;
        [SerializeField] private LocalizeStringEvent _nameLocalize;

        private RectTransform _rectTransform;
        private AsyncOperationHandle<Sprite> _handle;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Init(EquipmentInfo equipment)
        {
            _illustration.enabled = false;
            _headerBackground.color = equipment.Rarity.Color;
            _rarity.sprite = equipment.Rarity.Icon;
            _nftTag.SetActive(equipment.IsNftItem);
            _nameLocalize.StringReference = equipment.DisplayName;

            SetPosition();
            _handle = _illustration.LoadSpriteAndSet(equipment.Config.Image);
        }
        private void SetPosition()
        {
            var selectedGameObject = EventSystem.current.currentSelectedGameObject;
            if (selectedGameObject == null) return;
            var rectTransform = selectedGameObject.GetComponent<RectTransform>();
            if (rectTransform == null) return;
            _rectTransform.position = rectTransform.position;
            // random between 0 and 1;
            var pivotY = 0.5f;
            var pivotX = 0.5f;
            _rectTransform.pivot = new Vector2(pivotX, pivotY);
        }

        private void OnDisable()
        {
            if (_handle.IsValid()) Addressables.Release(_handle);
            _handle = default;
        }
    }
}