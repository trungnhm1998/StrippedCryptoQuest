using CryptoQuest.Battle.Components;
using CryptoQuest.Input;
using CryptoQuest.Menus.Status.States;
using CryptoQuest.Menus.Status.UI.Equipment;
using CryptoQuest.UI.Character;
using CryptoQuest.UI.Menu;
using FSM;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;

namespace CryptoQuest.Menus.Status.UI
{
    /// <summary>
    /// The context that hopefully holds all the UI information for the Status Menu. This is a mono behaviour class that
    /// can controls all the UI element or at least delegate back the reference to the correct state when needed.
    /// </summary>
    public class UIStatusMenu : UIMenuPanelBase
    {
        [field: SerializeField, Header("State Context")]
        public UICharacterEquipmentsPanel CharacterEquipmentsPanel { get; private set; }
        [field: SerializeField] public InputMediatorSO Input { get; private set; }

        [SerializeField] private UIEquipmentsInventory _equipmentsInventoryPanel;
        public UIEquipmentsInventory EquipmentsInventoryPanel => _equipmentsInventoryPanel;
        [field: SerializeField] public UIStatusCharacter CharacterPanel { get; private set; }
        [SerializeField] private AttributeChangeEvent _attributeChangeEvent;

        private StateMachine _stateMachine;

        private void Awake()
        {
            _stateMachine = new StatusMenuStateMachine(this);
        }

        private void OnEnable()
        {
            CharacterPanel.InspectingCharacter += InspectCharacter;
            _stateMachine.Init();
        }

        private void OnDisable()
        {
            _stateMachine.OnExit();
            CharacterPanel.InspectingCharacter -= InspectCharacter;
        }

        private void InspectCharacter(HeroBehaviour hero)
        {
            if (hero.IsValid() == false) return;
            _attributeChangeEvent.AttributeSystemReference = hero.GetComponent<AttributeSystemBehaviour>();
            // TODO: REFACTOR HERO EQUIPMENTS
            // CharacterEquipmentsPanel.SetEquipmentsUI(_inspectingHero.Equipments);
        }

        public void RequestStateChange(string state) => _stateMachine.RequestStateChange(state);
    }
}