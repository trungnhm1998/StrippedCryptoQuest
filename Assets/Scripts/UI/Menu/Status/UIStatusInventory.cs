using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Input;
using CryptoQuest.Menu;
using CryptoQuest.UI.Inventory;
using IndiGames.Core.Events.ScriptableObjects;
using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.UI.Menu.Status
{
    public class UIStatusInventory : MonoBehaviour, IRecyclableScrollRectDataSource
    {
        [Header("Configs")]
        [SerializeField] private RecyclableScrollRect _scrollRect;
        [SerializeField] private AutoScrollRect _autoScrollRect;
        [SerializeField] private InputMediatorSO _inputMediator;

        [Header("Events")]
        [SerializeField] private VoidEventChannelSO _confirmSelectEquipmentSlotEvent;
        [SerializeField] private VoidEventChannelSO _turnOffInventoryEvent;

        // TODO: REMOVE WHEN WE HAVE REAL DATA
        #region MOCK
        [Header("Mock")]
        [SerializeField] private int _itemCount;
        [SerializeField] private UIStatusMenuInventoryItem.Data _mockData;
        #endregion

        [Header("Game Components")]
        [SerializeField] private GameObject _contents;

        private List<UIStatusMenuInventoryItem.Data> _mockDataList = new();
        private UIStatusMenuInventoryItem _itemInformation;

        private void OnEnable()
        {
            _inputMediator.EnableStatusMenuInput();
            _confirmSelectEquipmentSlotEvent.EventRaised += ViewInventory;
        }


        private void OnDisable()
        {
            _confirmSelectEquipmentSlotEvent.EventRaised -= ViewInventory;
        }

        private void FixedUpdate()
        {
            _autoScrollRect.UpdateScrollRectTransform();

        }

        private void ViewInventory()
        {
            _contents.SetActive(true);
            RegisterInventoryInputEvents();

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
        }

        private void OnStatusMenuConfirmSelect()
        {
            _contents.SetActive(false);
            UnregisterInventoryInputEvents();
        }

        private void RegisterInventoryInputEvents()
        {
            // _inputMediator.St += SelectItemHandle;
            // _inputMediator.StatusMenuConfirmSelectEvent += OnStatusMenuConfirmSelect;
        }

        private void UnregisterInventoryInputEvents()
        {
            // _inputMediator.StatusMenuNavigateEvent -= SelectItemHandle;
            // _inputMediator.StatusMenuConfirmSelectEvent -= OnStatusMenuConfirmSelect;
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
            UIStatusMenuInventoryItem itemRow = cell as UIStatusMenuInventoryItem;
            itemRow.Init(_mockDataList[index], index);
        }
        #endregion
    }
}
