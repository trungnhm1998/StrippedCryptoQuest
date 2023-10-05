using System.Collections;
using System.Collections.Generic;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using CryptoQuest.UI.Menu.Panels.Status.Equipment;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.UI;
using CryptoQuest.UI.Menu.Panels.Status;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection
{
    public class UIEquipmentList : MonoBehaviour
    {
        [SerializeField] protected ScrollRect _scrollRect;
        [SerializeField] private GameObject _singleItemPrefab;
        [SerializeField] private RectTransform _tooltipSafeArea;

        private List<IData> _itemList = new List<IData>();
        private ITooltip _tooltip;

        private void Awake()
        {
            _tooltip = TooltipFactory.Instance.GetTooltip(ETooltipType.Equipment);
        }

        /// <summary>
        /// This method is a subscribed event and set up on scene.
        /// </summary>
        public void SetData(List<IData> data, bool isGameEquipmentListEmpty = false)
        {
            CleanUpScrollView();
            _itemList = data;
            RenderData();
            _tooltip.SetSafeArea(_tooltipSafeArea);
            CheckEmptyGameEquipmentList(isGameEquipmentListEmpty);
        }

        /// <summary>
        /// If the equipment list of Game board is not empty then the Wallet board will not run the SetDefaultSelection() method.
        /// </summary>
        /// <param name="isGameEquipmentListEmpty"></param>
        private void CheckEmptyGameEquipmentList(bool isGameEquipmentListEmpty = false)
        {
            if (!isGameEquipmentListEmpty) return;
            SetDefaultSelection();
        }

        /// <summary>
        /// This method subscribe to the HideDialogEvent and also is called when the switch-board event invokes on scene.
        /// </summary>
        public void SetDefaultSelection()
        {
            StartCoroutine(CoSetDefaultSelection());
        }

        /// <summary>
        /// Must delay this method a bit to let the scroll view finish initializing.
        /// </summary>
        private IEnumerator CoSetDefaultSelection()
        {
            yield return new WaitForSeconds(.1f);

            var firstButton = _scrollRect.content.GetComponentInChildren<MultiInputButton>();
            firstButton.Select();
        }

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
