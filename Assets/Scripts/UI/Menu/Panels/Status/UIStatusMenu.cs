using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
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
        [SerializeField] private ServiceProvider _provider;

        [field: SerializeField, Header("State Context")]
        public UICharacterEquipmentsPanel CharacterEquipmentsPanel { get; private set; }

        [field: SerializeField] public UIEquipmentsInventory EquipmentsInventoryPanel { get; private set; }
        [field: SerializeField] public UIStatusCharacter CharacterPanel { get; private set; }
        [SerializeField] private AttributeChangeEvent _attributeChangeEvent;
        private CharacterSpec _inspectingCharacter;
        public CharacterSpec InspectingCharacter => _inspectingCharacter; // Should have using ICharacter
        private IParty _party;

        private void Awake()
        {
            _party = _provider.PartyController.Party;
            CharacterPanel.InspectingCharacter += InspectCharacter;
            // This is event could not be fired because the scene contains this component is not loaded yet
            _provider.PartyProvided += BindParty;
        }

        private void OnDestroy()
        {
            CharacterPanel.InspectingCharacter -= InspectCharacter;
            _provider.PartyProvided -= BindParty;
        }

        private void BindParty(IPartyController partyController)
        {
            _party = partyController.Party;
        }

        private void InspectCharacter(int slotIdx)
        {
            var charInSlot = _party.Members[slotIdx];
            if (charInSlot.IsValid() == false) return;
            _inspectingCharacter = charInSlot;

            var charAttributeSystem = charInSlot.CharacterComponent.AttributeSystem;
            _attributeChangeEvent.AttributeSystemReference = charAttributeSystem;
            CharacterEquipmentsPanel.SetEquipmentsUI(_inspectingCharacter.Equipments);
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