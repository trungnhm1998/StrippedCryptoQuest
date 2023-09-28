using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Menu;
using CryptoQuest.UI.Menu.MenuStates.DimensionBoxStates;
using CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection;
using CryptoQuest.UI.Menu.Panels.DimensionBox.MetadTransferSection;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox
{
    public class UITransferTypeSelection : MonoBehaviour
    {
        public event UnityAction<UITransferSection> StartTransferEvent;

        [SerializeField] private MultiInputButton _defaultSelection;
        [SerializeField] private List<MultiInputButton> _transferTypeButtons = new();
        [SerializeField] private UIMetadSection _metadSection;
        [SerializeField] private UIEquipmentSection _equipmentSection;

        public UIDimensionBoxMenu MainPanel { get; set; }

        public void OpenEquipmentState() => MainPanel.Fsm.RequestStateChange(name: DimensionBoxMenuStateMachine.EquipmentTransfer);

        public void OpenMetadState() => MainPanel.Fsm.RequestStateChange(DimensionBoxMenuStateMachine.MetadTransfer);

        public void SetButtonsActive(bool enable)
        {
            foreach (var button in _transferTypeButtons)
            {
                button.interactable = enable;
            }
        }

        public void SetDefaultSelection() => _defaultSelection.Select();
    }
}
