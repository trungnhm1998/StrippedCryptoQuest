using CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection.Interfaces;
using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection
{
    public class UITransferItem : MonoBehaviour, ICell
    {
        public static event UnityAction<UITransferItem> SelectItemEvent;

        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private GameObject _pendingTag;
        [SerializeField] private GameObject _equippedTag;

        private bool _isSelected = false;

        public void ConfigureCell(IData itemInfo)
        {
            _icon.sprite = itemInfo.GetIcon();
            _name.StringReference = itemInfo.GetLocalizedName();
        }

        public void OnSelectToTransfer()
        {
            SelectItemEvent?.Invoke(this);

            _isSelected = !_isSelected;
            _pendingTag.SetActive(_isSelected);
        }

        public void Transfer(Transform parent)
        {
            gameObject.transform.SetParent(parent);
        }
    }
}
