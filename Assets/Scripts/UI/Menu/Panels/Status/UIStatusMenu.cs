using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.UI.Menu.MenuStates.StatusStates;
using CryptoQuest.UI.Menu.Panels.Status.Equipment;
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
        [SerializeField] private UIEquipmentOverview equipmentOverviewPanel;
        public UIEquipmentOverview EquipmentOverviewPanel => equipmentOverviewPanel;
        [field: SerializeField] public UIEquipmentList EquipmentListPanel { get; private set; }
        [field: SerializeField] public UIStatusCharacter CharacterPanel { get; private set; }

        public EEquipmentCategory EquippingType { get; set; }

        private IParty _party;
        private int _inspectingMemberIndex = 0;

        private void Awake()
        {
            _party = GetComponent<IParty>();
            if (_party == null) throw new NullReferenceException("Party is null");
            EquipmentListPanel.Init(_party);
            CharacterPanel.Init(_party);
        }

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