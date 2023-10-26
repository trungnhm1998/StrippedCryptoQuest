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
        [SerializeField] private UnityEvent<List<IGame>, bool> _setGameDataEvent;
        [SerializeField] private UnityEvent<List<INFT>, bool> _setWalletDataEvent;
        [SerializeField] private UnityEvent _hideDialogEvent;
        [SerializeField] private UnityEvent<Transform> _switchToWalletBoardEvent;
        [SerializeField] private UnityEvent<Transform> _switchToGameBoardEvent;

        private IGameEquipmentModel _gameModel;
        private IWalletEquipmentModel _walletModel;

        private List<IGame> _gameData = new();
        private List<INFT> _walletData = new();

        private List<uint> _selectedWalletEquipmentIds = new();
        public List<uint> SelectedWalletEquipmentIds { get => _selectedWalletEquipmentIds; private set => _selectedWalletEquipmentIds = value; }


        // This method subscribe to the _enterTransferSectionEvent on scene.
        public void StateEntered()
        {
            UITransferItem.SelectItemEvent += ItemSelected;
            _yesNoDialogEventSO.HideEvent += HideDialog;
            _uiEquipmentSection.SendingPhaseEvent += CheckIsOnSendingPhase;

            GetEquipments();
            StartCoroutine(CoSubscribeEventForHoveredItems());
        }

        // This method subscribe to the _exitTransferSectionEvent on scene.
        public void StateExited()
        {
            UITransferItem.SelectItemEvent -= ItemSelected;
            _yesNoDialogEventSO.HideEvent -= HideDialog;
            _uiEquipmentSection.SendingPhaseEvent -= CheckIsOnSendingPhase;

            UnSubscribeEventForHoveredItems();
            UnSubscribeInspectingEventForCurrentItem();
        }

        private void ItemSelected(UITransferItem currentItem)
        {
            Transform itemNewParent;
            var isGameboardAsCurrentParent = currentItem.Parent == _gameBoard;
            if (isGameboardAsCurrentParent)
            {
                itemNewParent = _walletBoard;
            }
            else
            {
                itemNewParent = _gameBoard;
                _selectedWalletEquipmentIds.Add(currentItem.Data.GetId());
            }

            currentItem.Transfer(itemNewParent);

            SetInteractableAllButtons(_gameBoard, !isGameboardAsCurrentParent);
            SetInteractableAllButtons(_walletBoard, isGameboardAsCurrentParent);
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
            _setWalletDataEvent.Invoke(_walletData, _walletData.Count <= 0 ? true : false);

            SetInteractableAllButtons(_walletBoard, false);
        }

        // This method subscribe to the _resetTransferEvent on scene.
        public void ResetTransfer()
        {
            if (_gameData.Count > 0)
                _setGameDataEvent.Invoke(_gameData, true);

            if (_walletData.Count > 0)
                _setWalletDataEvent.Invoke(_walletData, _gameData.Count <= 0 ? true : false);
        }

        // This method subscribe to the _switchBoardEvent on scene.
        public void SwitchBoardRequested(Vector2 direction)
        {
            SwitchToWalletBoard(direction);
            SwitchToGameBoard(direction);
        }

        private void SwitchToWalletBoard(Vector2 direction)
        {
            if (direction.x > 0 && _walletBoard.childCount > 0)
            {
                SetInteractableAllButtons(_gameBoard, false);
                SetInteractableAllButtons(_walletBoard, true);
                _switchToWalletBoardEvent.Invoke(_walletBoard);
            }
        }

        private void SwitchToGameBoard(Vector2 direction)
        {
            if (direction.x < 0 && _gameBoard.childCount > 0)
            {
                SetInteractableAllButtons(_walletBoard, false);
                SetInteractableAllButtons(_gameBoard, true);
                _switchToGameBoardEvent.Invoke(_gameBoard);
            }
        }

        private void SetInteractableAllButtons(Transform boardList, bool interactable)
        {
            foreach (Transform item in boardList)
            {
                item.GetComponent<MultiInputButton>().interactable = interactable;
            }
        }

        private IEnumerator CoSubscribeEventForHoveredItems()
        {
            yield return new WaitUntil(() => _gameBoard.childCount == _gameData.Count);
            foreach (Transform item in _gameBoard)
            {
                var itemUi = item.GetComponent<UITransferItem>();
                itemUi.CurrentHoveredItemEvent += SubscribeInspectingEventForCurrentItem;
            }
        }

        private void UnSubscribeEventForHoveredItems()
        {
            foreach (Transform item in _gameBoard)
            {
                var itemUi = item.GetComponent<UITransferItem>();
                itemUi.CurrentHoveredItemEvent -= SubscribeInspectingEventForCurrentItem;
            }
        }

        private UITransferItem _currentItem;
        private void SubscribeInspectingEventForCurrentItem(UITransferItem currentItem)
        {
            _currentItem = currentItem;
            _uiEquipmentSection.InspectItemEvent += _currentItem.ReceivedInspectingRequest;
        }
        
        private void UnSubscribeInspectingEventForCurrentItem()
        {
            _uiEquipmentSection.InspectItemEvent -= _currentItem.ReceivedInspectingRequest;
        }
    }
}
