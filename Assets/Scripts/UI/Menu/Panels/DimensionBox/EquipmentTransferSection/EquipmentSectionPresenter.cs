using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection
{
    public class EquipmentSectionPresenter : MonoBehaviour
    {
        [SerializeField] private UnityEvent<List<IData>> SetGameDataEvent;
        [SerializeField] private UnityEvent<List<IData>> SetWalletDataEvent;

        [SerializeField] private Transform _gameNftList;
        [SerializeField] private Transform _wallet;

        private IGameEquipmentModel _gameModel;
        private IWalletEquipmentModel _walletModel;

        private List<IData> _gameData = new();
        private List<IData> _walletData = new();

        private void Awake()
        {
            UITransferItem.SelectItemEvent += ItemSelected;
        }

        private void OnDestroy()
        {
            UITransferItem.SelectItemEvent -= ItemSelected;
        }

        public void GetEquipments()
        {
            StartCoroutine(GetGameEquipments());
            StartCoroutine(GetWalletEquipments());
        }

        private IEnumerator GetGameEquipments()
        {
            _gameModel = GetComponentInChildren<IGameEquipmentModel>();
            yield return _gameModel.CoGetData();
            _gameData = _gameModel.Data;
            SetGameDataEvent.Invoke(_gameData);
        }

        private IEnumerator GetWalletEquipments()
        {
            _walletModel = GetComponentInChildren<IWalletEquipmentModel>();
            yield return _walletModel.CoGetData();
            _walletData = _walletModel.Data;
            SetWalletDataEvent.Invoke(_walletData);
        }

        private void ItemSelected(UITransferItem currentItem)
        {
            var itemParent = currentItem.Parent == _gameNftList ? _wallet : _gameNftList;
            currentItem.Transfer(itemParent);
        }

        public void ResetTransfer()
        {
            SetGameDataEvent.Invoke(_gameData);
            SetWalletDataEvent.Invoke(_walletData);
        }
    }
}
