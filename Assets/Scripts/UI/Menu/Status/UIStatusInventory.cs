using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Input;
using CryptoQuest.Menu;
using CryptoQuest.UI.Inventory;
using IndiGames.Core.Events.ScriptableObjects;
using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.UI.Menu.Status
{
    public class UIStatusInventory : MonoBehaviour, IRecyclableScrollRectDataSource
    {
        [Header("Configs")]
        [SerializeField] private RecyclableScrollRect _scrollRect;
        [SerializeField] private InputMediatorSO _inputMediator;

        [Header("Events")]
        [SerializeField] private VoidEventChannelSO _confirmSelectEquipmentSlotEvent;
        [SerializeField] private VoidEventChannelSO _turnOffInventoryEvent;

        // TODO: REMOVE WHEN WE HAVE REAL DATA
        #region MOCK
        [Header("Mock")]
        [SerializeField] private int _itemCount;
        [SerializeField] private UIStatusInventoryItem.Data _mockData;
        #endregion

        [Header("Game Components")]
        [SerializeField] private GameObject _contents;
        [SerializeField] private RectTransform _myScrollRect;
        [SerializeField] private RectTransform _singleItemRect;

        private List<UIStatusInventoryItem.Data> _mockDataList = new();
        private UIStatusInventoryItem _itemInformation;
        private float _verticalOffset;

        private void OnEnable()
        {
            _confirmSelectEquipmentSlotEvent.EventRaised += ViewInventory;
        }


        private void OnDisable()
        {
            _confirmSelectEquipmentSlotEvent.EventRaised -= ViewInventory;
        }

        private void ViewInventory()
        {
            _contents.SetActive(true);
            _inputMediator.EnableStatusEquipmentsInventoryInput();
            RegisterInventoryInputEvents();

            _verticalOffset = _singleItemRect.rect.height;

            // TODO: REMOVE WHEN WE HAVE REAL DATA
            for (int i = 0; i < _itemCount; i++)
            {
                _mockDataList.Add(_mockData.Clone());
            }

            _scrollRect.Initialize(this);
        }

        private void OnTurnOffInventory()
        {
            _turnOffInventoryEvent.RaiseEvent();
            _contents.SetActive(false);
            _inputMediator.EnableStatusEquipmentsInput();
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

        private void RegisterInventoryInputEvents()
        {
            _inputMediator.StatusEquipmentInventoryCancelEvent += OnTurnOffInventory;
            _inputMediator.StatusInventoryGoBelowEvent += StatusInventoryGoDown;
            _inputMediator.StatusInventoryGoAboveEvent += StatusInventoryGoUp;
        }

        private void UnregisterInventoryInputEvents()
        {
            _inputMediator.StatusEquipmentInventoryCancelEvent -= OnTurnOffInventory;
            _inputMediator.StatusInventoryGoBelowEvent -= StatusInventoryGoDown;
            _inputMediator.StatusInventoryGoAboveEvent -= StatusInventoryGoUp;
        }

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
