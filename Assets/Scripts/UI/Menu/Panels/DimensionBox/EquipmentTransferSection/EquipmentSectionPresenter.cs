using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Events.UI.Dialogs;
using CryptoQuest.Menu;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection
{
    public class EquipmentSectionPresenter : MonoBehaviour
    {
        [SerializeField] private YesNoDialogEventChannelSO _yesNoDialogEventSO;
        [SerializeField] private UIEquipmentSection _uiEquipmentSection;
        [SerializeField] private Transform _gameBoard;
        [SerializeField] private Transform _walletBoard;

        [Header("Events set up on scene")]
        [SerializeField] private UnityEvent<List<IData>, bool> _setGameDataEvent;
        [SerializeField] private UnityEvent<List<IData>, bool> _setWalletDataEvent;
        [SerializeField] private UnityEvent _hideDialogEvent;
        [SerializeField] private UnityEvent _switchToWalletBoardEvent;
        [SerializeField] private UnityEvent _switchToGameBoardEvent;

        private IGameEquipmentModel _gameModel;
        private IWalletEquipmentModel _walletModel;

        private List<IData> _gameData = new();
        private List<IData> _walletData = new();


        // This method subscribe to the EnterTransferSectionEvent on scene.
        public void StateEntered()
        {
            UITransferItem.SelectItemEvent += ItemSelected;
            _yesNoDialogEventSO.HideEvent += HideDialog;
            _uiEquipmentSection.SendingPhaseEvent += CheckIsOnSendingPhase;

            GetEquipments();
        }

        // This method subscribe to the ExitTransferSectionEvent on scene.
        public void StateExited()
        {
            UITransferItem.SelectItemEvent -= ItemSelected;
            _yesNoDialogEventSO.HideEvent -= HideDialog;
            _uiEquipmentSection.SendingPhaseEvent -= CheckIsOnSendingPhase;
        }

        private void ItemSelected(UITransferItem currentItem)
        {
            var itemParent = currentItem.Parent == _gameBoard ? _walletBoard : _gameBoard;
            currentItem.Transfer(itemParent);
        }

        private void HideDialog()
        {
            _hideDialogEvent.Invoke();
        }

        private void CheckIsOnSendingPhase(bool isOnSendingPhase)
        {
            SetInteractableAllButtons(_gameBoard, !isOnSendingPhase);
            SetInteractableAllButtons(_walletBoard, !isOnSendingPhase);
        }

        private void GetEquipments()
        {
            StartCoroutine(GetGameEquipments());
            StartCoroutine(GetWalletEquipments());
            SetInteractableAllButtons(_walletBoard, false);
        }

        private IEnumerator GetGameEquipments()
        {
            _gameModel = GetComponentInChildren<IGameEquipmentModel>();
            yield return _gameModel.CoGetData();

            if (_gameModel.Data.Count <= 0) yield break;

            _gameData = _gameModel.Data;
            _setGameDataEvent.Invoke(_gameData, true);
        }

        private IEnumerator GetWalletEquipments()
        {
            _walletModel = GetComponentInChildren<IWalletEquipmentModel>();
            yield return _walletModel.CoGetData();
            yield return new WaitUntil(() => _walletModel.IsLoaded);

            if (_walletModel.Data.Count <= 0) yield break;

            _walletData = _walletModel.Data;
            _setWalletDataEvent.Invoke(_walletData, _gameData.Count <= 0 ? true : false);
        }

        /// <summary>
        /// This method subscribe to the ResetTransferEvent on scene.
        /// </summary>
        public void ResetTransfer()
        {
            if (_gameData.Count > 0)
                _setGameDataEvent.Invoke(_gameData, true);

            if (_walletData.Count > 0)
                _setWalletDataEvent.Invoke(_walletData, _gameData.Count <= 0 ? true : false);
        }

        /// <summary>
        /// This method subscribe to the SwitchBoardEvent on scene.
        /// </summary>
        public void SwitchBoardRequested(Vector2 direction)
        {
            SwitchToWalletBoard(direction);
            SwitchToGameBoard(direction);
        }

        private void SwitchToWalletBoard(Vector2 direction)
        {
            if (direction.x > 0)
            {
                SetInteractableAllButtons(_gameBoard, false);
                SetInteractableAllButtons(_walletBoard, true);
                _switchToWalletBoardEvent.Invoke();
            }
        }

        private void SwitchToGameBoard(Vector2 direction)
        {
            if (direction.x < 0)
            {
                SetInteractableAllButtons(_walletBoard, false);
                SetInteractableAllButtons(_gameBoard, true);
                _switchToGameBoardEvent.Invoke();
            }
        }

        private void SetInteractableAllButtons(Transform boardList, bool interactable)
        {
            foreach (Transform item in boardList)
            {
                item.GetComponent<MultiInputButton>().interactable = interactable;
            }
        }
    }
}
