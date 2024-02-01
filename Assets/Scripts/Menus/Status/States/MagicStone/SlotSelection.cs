using System.Collections.Generic;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Menus.Status.UI;
using CryptoQuest.Menus.Status.UI.MagicStone;
using CryptoQuest.Sagas.Equipment;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Menus.Status.States.MagicStone
{
    public class SlotSelection : StatusStateBase
    {
        private UIEquipmentDetails _uiEquipmentDetails;
        private TinyMessageSubscriptionToken _equipmentUpdatedToken;

        public SlotSelection(UIStatusMenu statusPanel) : base(statusPanel)
        {
        }

        public override void OnEnter()
        {
            StatusPanel.Input.MenuCancelEvent += BackToEquipmentSelection;
            StatusPanel.UIAttachList.AttachSlotSelectedEvent += ToNavigatingBetweenElements;
            _equipmentUpdatedToken = ActionDispatcher.Bind<EquipmentUpdated>(DetachedStone);

            _uiEquipmentDetails = StatusPanel.EquipmentDetails;
            StatusPanel.MagicStoneSelection.SetActiveAllElementButtons(false);

            StatusPanel.MagicStoneSelection.UIStoneList.SetActiveAllStoneButtons(false);
            StatusPanel.UIAttachList.EnterSlotSelection();
            StatusPanel.UIAttachList.RenderCurrentAttachedStones(_uiEquipmentDetails.Equipment);
        }

        public override void OnExit()
        {
            StatusPanel.UIAttachList.ExitSlotSelection();
            StatusPanel.Input.MenuCancelEvent -= BackToEquipmentSelection;
            StatusPanel.UIAttachList.AttachSlotSelectedEvent -= ToNavigatingBetweenElements;
            StatusPanel.MagicStoneSelection.DeactivateTooltip();
            ActionDispatcher.Unbind(_equipmentUpdatedToken);
        }

        private void ToNavigatingBetweenElements(IMagicStone stoneData)
        {
            if (stoneData == null)
            {
                fsm.RequestStateChange(State.MAGIC_STONE_ELEMENT_NAVIGATION);
                return;
            }

            CallDetachAPI(stoneData);
        }

        private void DetachedStone(EquipmentUpdated ctx)
        {
            StatusPanel.UIAttachList.DetachCurrent();
            StatusPanel.MagicStoneSelection.UIStoneList.RenderAll();
            fsm.RequestStateChange(State.MAGIC_STONE_ELEMENT_NAVIGATION);
        }

        private void CallDetachAPI(IMagicStone stoneData)
        {
            if (stoneData == null) return;
            List<int> stoneIDs = new();
            Debug.Log($"<color=white>CallDetachAPI::stoneIDs={stoneIDs}</color>");
            stoneIDs.Add(stoneData.ID);
            ActionDispatcher.Dispatch(new DetachStones()
            {
                EquipmentID = _uiEquipmentDetails.Equipment.Id,
                StoneIDs = stoneIDs
            });
        }

        private void BackToEquipmentSelection()
        {
            StatusPanel.MagicStoneMenu.SetActive(false);
            StatusPanel.BackToPreviousState();
        }
    }
}