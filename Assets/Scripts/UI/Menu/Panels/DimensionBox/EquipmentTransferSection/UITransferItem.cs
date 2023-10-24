using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using CryptoQuest.UI.Menu.Panels.Status;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection
{
    public class UITransferItem : MonoBehaviour
    {
        public static event UnityAction<UITransferItem> SelectItemEvent;

        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private GameObject _pendingTag;
        [SerializeField] private GameObject _equippedTag;

        [Header("Tooltip")]
        [SerializeField] private RectTransform _tooltipPosition;

        private Transform _parent;
        public Transform Parent { get => _parent; set => _parent = value; }

        private bool _isSelected = false;
        private bool _isEquipped = false;
        private ITooltip _tooltip;
        private bool _isShowTooltip = false;

        public IData Data { get; private set; }

        private void Awake()
        {
            _tooltip = TooltipFactory.Instance.GetTooltip(ETooltipType.Equipment);
            UIEquipmentSection.InspectItemEvent += ReceivedInspectingRequest;
        }

        private void OnDestroy()
        {
            UIEquipmentSection.InspectItemEvent -= ReceivedInspectingRequest;
        }

        public void ConfigureCell(INFT itemInfo)
        {
            Data = itemInfo;
            SetDataToUI(itemInfo);
        }

        public void ConfigureCell(IGame itemInfo)
        {
            Data = itemInfo;
            SetDataToUI(itemInfo);

            _isEquipped = itemInfo.IsEquipped();
            _equippedTag.SetActive(_isEquipped);
        }

        private void SetDataToUI(IData itemInfo)
        {
            _icon.sprite = itemInfo.GetIcon();
            _name.StringReference = itemInfo.GetLocalizedName();
        }

        public void OnSelectToTransfer()
        {
            if (_isEquipped) return;
            SelectItemEvent?.Invoke(this);

            _isSelected = !_isSelected;
            _pendingTag.SetActive(_isSelected);
        }

        public void Transfer(Transform parent)
        {
            gameObject.transform.SetParent(parent);
            _parent = parent;
        }

        public void OnInspecting(bool isInspecting)
        {
            if (isInspecting == false)
            {
                _tooltip.Hide();
                return;
            }

            _tooltip
                .WithLevel(1)
                .WithDisplaySprite(_icon.sprite)
                .WithContentAwareness(_tooltipPosition);
        }

        public void ReceivedInspectingRequest()
        {
            _isShowTooltip = !_isShowTooltip;

            if (_isShowTooltip) _tooltip.Show();
            else _tooltip.Hide();
        }
    }
}
