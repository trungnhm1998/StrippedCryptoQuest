using System.Collections;
using System.Collections.Generic;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using CryptoQuest.UI.Menu.Panels.Status.Equipment;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection
{
    public abstract class UIEquipmentList : MonoBehaviour
    {
        [SerializeField] protected ScrollRect _scrollRect;
        [SerializeField] private GameObject _singleItemPrefab;
        [SerializeField] private TooltipProvider _tooltipProvider;
        [SerializeField] private RectTransform _tooltipSafeArea;

        private List<IData> _itemList = new List<IData>();

        public void SetData(List<IData> data)
        {
            CleanUpScrollView();
            _itemList = data;
            RenderData();
            Invoke(nameof(SetDefaultSelection), .1f);
            _tooltipProvider.Tooltip.SetSafeArea(_tooltipSafeArea);
        }

        /// <summary>
        /// Must delay this method a bit to let the scroll view finish initializing.
        /// </summary>
        protected virtual void SetDefaultSelection() { }

        protected virtual void CleanUpScrollView()
        {
            foreach (Transform child in _scrollRect.content)
            {
                Destroy(child.gameObject);
            }
        }

        protected virtual void SetParentIdentity(UITransferItem item)
        {
            item.Parent = _scrollRect.content;
        }

        protected virtual void RenderData()
        {
            foreach (var itemData in _itemList)
            {
                var item = Instantiate(_singleItemPrefab, _scrollRect.content).GetComponent<UITransferItem>();
                item.ConfigureCell(itemData);
                SetParentIdentity(item);
            }
        }
    }
}
