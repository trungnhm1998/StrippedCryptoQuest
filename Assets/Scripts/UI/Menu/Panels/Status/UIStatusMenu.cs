using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using CryptoQuest.UI.Character;
using CryptoQuest.UI.Menu.MenuStates.StatusStates;
using CryptoQuest.UI.Menu.Panels.Status.Equipment;
using FSM;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
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

        [SerializeField] private UIEquipmentsInventory _equipmentsInventoryPanel;
        public UIEquipmentsInventory EquipmentsInventoryPanel => _equipmentsInventoryPanel;
        [field: SerializeField] public UIStatusCharacter CharacterPanel { get; private set; }
        [SerializeField] private AttributeChangeEvent _attributeChangeEvent;

        private void Awake()
        {
            CharacterPanel.InspectingCharacter += InspectCharacter;
        }

        private void OnDestroy()
        {
            CharacterPanel.InspectingCharacter -= InspectCharacter;
        }

        private void InspectCharacter(HeroBehaviour hero)
        {
            if (hero.IsValid() == false) return;
            _attributeChangeEvent.AttributeSystemReference = hero.GetComponent<AttributeSystemBehaviour>();
            // TODO: REFACTOR HERO EQUIPMENTS
            // CharacterEquipmentsPanel.SetEquipmentsUI(_inspectingHero.Equipments);
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