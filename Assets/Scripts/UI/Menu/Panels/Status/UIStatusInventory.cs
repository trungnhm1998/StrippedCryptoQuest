using System.Collections.Generic;
using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.UI.Menu.Panels.Status
{
    public class UIStatusInventory : MonoBehaviour, IRecyclableScrollRectDataSource
    {
        [Header("Configs")]
        [SerializeField] private RecyclableScrollRect _scrollRect;

        // TODO: REMOVE WHEN WE HAVE REAL DATA

        #region MOCK

        [Header("Mock")]
        [SerializeField] private int _itemCount;

        [FormerlySerializedAs("_mockData")] [SerializeField]
        private UIStatusInventoryItem.MockData mockMockData;

        #endregion

        [Header("Game Components")]
        [SerializeField] private GameObject _contents;

        [SerializeField] private RectTransform _myScrollRect;
        [SerializeField] private RectTransform _singleItemRect;

        private List<UIStatusInventoryItem.MockData> _mockDataList = new();

        private float _verticalOffset;
        private bool _initialized = false;

        public void Show(UIEquipmentSlotButton.EEquipmentType statusPanelEquippingType)
        {
            Debug.Log($"Show equipments with type [{statusPanelEquippingType}]");
            _contents.SetActive(true);
            RegisterInventoryInputEvents();

            _verticalOffset = _singleItemRect.rect.height;

            // TODO: REMOVE WHEN WE HAVE REAL DATA
            for (int i = 0; i < _itemCount; i++)
            {
                _mockDataList.Add(mockMockData.Clone());
            }

            // only init after get data
            if (!_initialized)
            {
                _initialized = true;
                _scrollRect.Initialize(this);
            }
        }

        public void Hide()
        {
            _contents.SetActive(false);
            UnregisterInventoryInputEvents();
        }

        private void StatusInventoryGoDown()
        {
            Scroll(-_verticalOffset);
        }

        private void StatusInventoryGoUp()
        {
            Scroll(_verticalOffset);
        }

        private void Scroll(float value)
        {
            _myScrollRect.anchoredPosition -= new Vector2(0, value);
        }


        private void RegisterInventoryInputEvents() { }

        private void UnregisterInventoryInputEvents() { }

        #region PLUGINS

        /// <summary>
        /// needed for plugins
        /// </summary>
        /// <returns>Real data count</returns>
        public int GetItemCount()
        {
            return _mockDataList.Count;
        }

        /// <summary>
        /// Will be called auto
        /// </summary>
        /// <param name="cell">The prefab that we can cast to set Data to UI</param>
        /// <param name="index">query from real data using this index</param>
        public void SetCell(ICell cell, int index)
        {
            UIStatusInventoryItem itemRow = cell as UIStatusInventoryItem;
            itemRow.Init(_mockDataList[index], index);
        }

        #endregion
    }
}