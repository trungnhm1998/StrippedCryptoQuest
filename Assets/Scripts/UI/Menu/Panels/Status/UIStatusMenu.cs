using System;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.UI.Character;
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
        [field: SerializeField, Header("State Context")]
        public UICharacterEquipmentsPanel CharacterEquipmentsPanel { get; private set; }

        [field: SerializeField] public UIEquipmentsInventory EquipmentsInventoryPanel { get; private set; }
        [field: SerializeField] public UIStatusCharacter CharacterPanel { get; private set; }

        [SerializeField] private AttributeChangeEvent _attributeChangeEvent;

        private IParty _party;

        private void Awake()
        {
            _party = GetComponent<IParty>();
            if (_party == null) throw new NullReferenceException("Party is null");
            CharacterPanel.SetParty(_party);

            CharacterPanel.InspectingCharacter += InspectCharacter;
        }

        private void OnDestroy()
        {
            CharacterPanel.InspectingCharacter -= InspectCharacter;
        }

        private void InspectCharacter(int slotIdx)
        {
            var charInSlot = _party.Members[slotIdx];
            if (charInSlot.IsValid() == false) return;

            var charAttributeSystem = charInSlot.CharacterComponent.AttributeSystem;
            _attributeChangeEvent.AttributeSystemReference = charAttributeSystem;
            charAttributeSystem.UpdateAttributeValues(); // event will be raise even though the value is the same
            CharacterEquipmentsPanel.SetEquipment(charInSlot.Equipments);
        }

        private StatusMenuStateMachine _state;
        public StatusMenuStateMachine State => _state;

        /// <summary>
        /// Return the specific state machine for this panel.
        /// </summary>
        /// <param name="menuManager"></param>
        /// <returns>The <see cref="StatusMenuStateMachine"/> which derived
        /// <see cref="CryptoQuest.UI.Menu.MenuStates.MenuStateMachine"/> derived
        /// from <see cref="StateMachine"/> which also derived from <see cref="StateBase"/></returns>
        public override StateBase<string> GetPanelState(MenuManager menuManager)
        {
            return _state ??= new StatusMenuStateMachine(this);
        }
    }
}