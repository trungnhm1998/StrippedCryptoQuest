using CryptoQuest.UI.Menu.MenuStates.StatusStates;
using FSM;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Status
{
    /// <summary>
    /// The context that hopefully holds all the UI information for the Status Menu. This is a mono behaviour class that
    /// can controls all the UI element or at least delegate back the reference to the correct state when needed.
    /// </summary>
    public class UIStatusMenu : UIMenuPanel
    {
        [Header("State Context")]
        [SerializeField] private UIStatusMenuEquipment _equipmentOverviewPanel;
        public UIStatusMenuEquipment EquipmentOverviewPanel => _equipmentOverviewPanel;
        [field: SerializeField] public UIStatusInventory InventoryPanel { get; private set; }

        public UIEquipmentSlotButton.EEquipmentType EquippingType { get; set; }


        /// <summary>
        /// Return the specific state machine for this panel.
        /// </summary>
        /// <param name="menuManager"></param>
        /// <returns>The <see cref="StatusMenuStateMachine"/> which derived
        /// <see cref="CryptoQuest.UI.Menu.MenuStates.MenuStateMachine"/> derived
        /// from <see cref="StateMachine"/> which also derived from <see cref="StateBase"/></returns>
        public override StateBase<string> GetPanelState(MenuManager menuManager)
        {
            return new StatusMenuStateMachine(this);
        }
    }
}