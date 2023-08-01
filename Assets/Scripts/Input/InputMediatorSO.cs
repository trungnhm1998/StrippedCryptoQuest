using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace CryptoQuest.Input
{
    // TODO: Move action map interfaces to separate scriptable objects
    public class InputMediatorSO : ScriptableObject,
        InputActions.IMapGameplayActions,
        InputActions.IMenusActions,
        InputActions.IDialoguesActions,
        InputActions.IHomeMenuActions,
        InputActions.IStatusMenuActions,
        InputActions.IStatusEquipmentsActions,
        InputActions.IStatusEquipmentsInventoryActions,
        InputActions.IStatusMagicStoneActions
    {
        #region Events

        #region Gameplay

        public event UnityAction<Vector2> MoveEvent;
        public event UnityAction PauseEvent;
        public event UnityAction InteractEvent;
        public event UnityAction OpenMainMenuEvent;

        #endregion

        #region Menu

        public event UnityAction MenuConfirmedEvent;
        public event UnityAction MenuSubmitEvent;
        public event UnityAction MenuNavigateEvent;
        public event UnityAction MouseMoveEvent;
        public event UnityAction MenuTabPressed;
        public event UnityAction CancelEvent;
        public event UnityAction HomeMenuSortEvent;

        public event UnityAction NextSelectionMenu;
        public event UnityAction PreviousSelectionMenu;

        #endregion

        #region Dialogue

        public event UnityAction NextDialoguePressed;

        #endregion

        #region HomeMenu

        public event UnityAction NextEvent;
        public event UnityAction PreviousEvent;
        public event UnityAction ConfirmEvent;
        public event UnityAction HomeMenuCancelEvent;

        #endregion

        #region StatusMenu

        public event UnityAction EnableChangeEquipmentModeEvent;
        public event UnityAction StatusEquipmentGoBelowEvent;
        public event UnityAction StatusEquipmentGoAboveEvent;
        public event UnityAction StatusMenuConfirmSelectEvent;
        public event UnityAction StatusMenuCancelEvent;
        public event UnityAction StatusEquipmentCancelEvent;
        public event UnityAction StatusEquipmentInventoryCancelEvent;
        public event UnityAction StatusInventoryGoBelowEvent;
        public event UnityAction StatusInventoryGoAboveEvent;
        public event UnityAction EnableMagicStoneMenuEvent;
        public event UnityAction TurnOffMagicStoneMenuEvent;

        #endregion

        #endregion

        private InputActions _inputActions;
        public InputActions InputActions => _inputActions;

        private void OnEnable()
        {
            CreateInputInstanceIfNeeded();

            _inputActions.Disable();
        }

        private void OnDisable()
        {
            DisableAllInput();
        }

        #region Main

        public void DisableAllInput()
        {
            _inputActions.Disable();
            _inputActions.MapGameplay.Disable();
            _inputActions.Menus.Disable();
            _inputActions.Dialogues.Disable();
            _inputActions.HomeMenu.Disable();
            _inputActions.StatusMenu.Disable();
            _inputActions.StatusEquipments.Disable();
            _inputActions.StatusEquipmentsInventory.Disable();
            _inputActions.StatusMagicStone.Disable();
        }

        private void CreateInputInstanceIfNeeded()
        {
            if (_inputActions != null) return;

            _inputActions = new InputActions();
            _inputActions.Menus.SetCallbacks(this);
            _inputActions.MapGameplay.SetCallbacks(this);
            _inputActions.Dialogues.SetCallbacks(this);
            _inputActions.HomeMenu.SetCallbacks(this);
            _inputActions.StatusMenu.SetCallbacks(this);
            _inputActions.StatusEquipments.SetCallbacks(this);
            _inputActions.StatusEquipmentsInventory.SetCallbacks(this);
            _inputActions.StatusMagicStone.SetCallbacks(this);
        }

        public void EnableMenuInput()
        {
            DisableAllInput();
            _inputActions.Menus.Enable();
        }

        public void EnableDialogueInput()
        {
            DisableAllInput();
            _inputActions.Dialogues.Enable();
        }

        public void EnableMapGameplayInput()
        {
            _inputCached.Clear();
            DisableAllInput();
            _inputActions.MapGameplay.Enable();
        }

        public void EnableHomeMenuInput()
        {
            DisableAllInput();
            _inputActions.HomeMenu.Enable();
        }

        public void EnableStatusMenuInput()
        {
            DisableAllInput();
            _inputActions.StatusMenu.Enable();
        }

        public void EnableStatusEquipmentsInput()
        {
            DisableAllInput();
            _inputActions.StatusEquipments.Enable();
        }
        
        public void EnableStatusEquipmentsInventoryInput()
        {
            DisableAllInput();
            _inputActions.StatusEquipmentsInventory.Enable();
        }
        
        public void EnableStatusMagicStoneInput()
        {
            DisableAllInput();
            _inputActions.StatusMagicStone.Enable();
        }

        #endregion

        #region MapGameplayActions

        private List<Vector2> _inputCached = new();

        private void GameplayRemoveMove(Vector2 direction)
        {
            _inputCached.Remove(direction);

            if (_inputCached.Count <= 0)
            {
                MoveEvent?.Invoke(Vector3.zero);
                return;
            }
            MoveEvent?.Invoke(_inputCached[_inputCached.Count - 1]);
        }

        private void GameplayMove(Vector2 direction)
        {
            if (!_inputCached.Contains(direction))
            {
                _inputCached.Add(direction);
            }
            MoveEvent?.Invoke(_inputCached[_inputCached.Count - 1]);
        }

        public void OnMoveUp(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                GameplayRemoveMove(Vector2.up);
                return;
            }
            GameplayMove(Vector2.up);
        }
        public void OnMoveDown(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                GameplayRemoveMove(Vector2.down);
                return;
            }
            GameplayMove(Vector2.down);
        }
        public void OnMoveLeft(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                GameplayRemoveMove(Vector2.left);
                return;
            }
            GameplayMove(Vector2.left);
        }
        public void OnMoveRight(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                GameplayRemoveMove(Vector2.right);
                return;
            }
            GameplayMove(Vector2.right);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed) InteractEvent?.Invoke();
        }

        public void OnMainMenu(InputAction.CallbackContext context)
        {
            if (context.performed) OpenMainMenuEvent?.Invoke();
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed) PauseEvent?.Invoke();
        }

        #endregion

        #region MenuActions

        public void OnNavigate(InputAction.CallbackContext context)
        {
            if (context.performed) MenuNavigateEvent?.Invoke();
        }

        public void OnConfirm(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                MenuConfirmedEvent?.Invoke();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.performed) CancelEvent?.Invoke();
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            if (context.performed) MenuSubmitEvent?.Invoke();
        }

        public bool LeftMouseDown() => Mouse.current.leftButton.isPressed;
        public void OnClick(InputAction.CallbackContext context) { }

        public void OnPoint(InputAction.CallbackContext context) { }

        public void OnNextSelection(InputAction.CallbackContext context)
        {
            if (context.performed) MenuTabPressed?.Invoke();
        }

        public void OnMouseMove(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) MouseMoveEvent?.Invoke();
        }

        public void OnNextSelectionMenu(InputAction.CallbackContext context)
        {
            if (context.performed) NextSelectionMenu?.Invoke();
        }

        public void OnPreviousSelectionMenu(InputAction.CallbackContext context)
        {
            if (context.performed) PreviousSelectionMenu?.Invoke();
        }

        public void OnHomeMenuEnableSort(InputAction.CallbackContext context)
        {
            if (context.performed) HomeMenuSortEvent?.Invoke();
        }

        #endregion

        #region Dialogue

        public void OnNextDialogue(InputAction.CallbackContext context)
        {
            if (context.performed) NextDialoguePressed?.Invoke();
        }

        public void OnEscape(InputAction.CallbackContext context)
        {
            Debug.LogWarning("Escape pressed, but not implemented");
        }


        #endregion

        #region HomeMenu

        public void OnNext(InputAction.CallbackContext context)
        {
            if (context.performed) NextEvent?.Invoke();
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            if (context.performed) PreviousEvent?.Invoke();
        }

        public void OnConfirmSelect(InputAction.CallbackContext context)
        {
            if (context.performed) ConfirmEvent?.Invoke();
        }

        public void OnHomeMenuCancel(InputAction.CallbackContext context)
        {
            if (context.performed) HomeMenuCancelEvent?.Invoke();
        }

        #endregion

        #region StatusMenu
        public void OnEnableChangeEquipmentMode(InputAction.CallbackContext context)
        {
            if (context.performed) EnableChangeEquipmentModeEvent?.Invoke();
        }

        public void OnStatusEquipmentCancel(InputAction.CallbackContext context)
        {
            if (context.performed) StatusEquipmentCancelEvent?.Invoke();
        }

        public void OnCharacterChange(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region StatusMenu.Equipments
        public void OnStatusMenuConfirmSelect(InputAction.CallbackContext context)
        {
            if (context.performed) StatusMenuConfirmSelectEvent?.Invoke();
        }
        
        public void OnGoBelow(InputAction.CallbackContext context)
        {
            if (context.performed) StatusEquipmentGoBelowEvent?.Invoke();
        }

        public void OnGoAbove(InputAction.CallbackContext context)
        {
            if (context.performed) StatusEquipmentGoAboveEvent?.Invoke();
        }

        public void OnMagicStone(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnStatusMenuCancel(InputAction.CallbackContext context)
        {
            if (context.performed) StatusMenuCancelEvent?.Invoke();
        }
        public void OnEnableMagicStoneMenu(InputAction.CallbackContext context)
        {
            if (context.performed) EnableMagicStoneMenuEvent?.Invoke();
        }
        #endregion

        #region StatusMenu.Equipments.Inventory
        public void OnStatusInventoryCancel(InputAction.CallbackContext context)
        {
            if (context.performed) StatusEquipmentInventoryCancelEvent?.Invoke();
        }

        public void OnStatusInventoryGoBelow(InputAction.CallbackContext context)
        {
            if (context.performed) StatusInventoryGoBelowEvent?.Invoke();
        }

        public void OnStatusInventoryGoAbove(InputAction.CallbackContext context)
        {
            if (context.performed) StatusInventoryGoAboveEvent?.Invoke();
        }
        #endregion

        #region StatusMenu.MagicStone
        public void OnTurnOffMagicStoneMenu(InputAction.CallbackContext context)
        {
            if (context.performed) TurnOffMagicStoneMenuEvent?.Invoke();
        }
        #endregion
    }
}