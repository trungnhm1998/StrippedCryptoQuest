using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Input;
using IndiGames.Core.Events.ScriptableObjects;
using PolyAndCode.UI;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.UI.Menu.Status
{
    public class UIStatusInventory : MonoBehaviour, IRecyclableScrollRectDataSource
    {
        [Header("Configs")]
        [SerializeField] private RecyclableScrollRect _scrollRect;
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private VoidEventChannelSO _confirmSelectEquipmentSlotEvent;

        // TODO: REMOVE WHEN WE HAVE REAL DATA
        #region MOCK
        [Header("Mock")]
        [SerializeField] private int _itemCount;
        [SerializeField] private UIStatusMenuInventoryItem.Data _mockData;
        #endregion

        [Header("Game Components")]
        [SerializeField] private GameObject _contents;

        private List<UIStatusMenuInventoryItem.Data> _mockDataList = new();

        private void OnEnable()
        {
            _inputMediator.EnableStatusMenuInput();
            _confirmSelectEquipmentSlotEvent.EventRaised += ViewInventory;
            _inputMediator.StatusMenuCancelEvent += TurnOffInventory;
        }

        private void OnDisable()
        {
            _inputMediator.EnableStatusMenuInput();
            _confirmSelectEquipmentSlotEvent.EventRaised -= ViewInventory;
            _inputMediator.StatusMenuCancelEvent -= TurnOffInventory;
        }

        private void ViewInventory()
        {
            _contents.SetActive(true);

            // TODO: REMOVE WHEN WE HAVE REAL DATA
            for (int i = 0; i < _itemCount; i++)
            {
                _mockDataList.Add(_mockData.Clone());
            }

            _scrollRect.Initialize(this);
        }

        private void TurnOffInventory()
        {
            _contents.SetActive(false);
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
            UIStatusMenuInventoryItem equipmentRow = cell as UIStatusMenuInventoryItem;
            equipmentRow.Init(_mockDataList[index], index);
        }
        #endregion
    }
}
