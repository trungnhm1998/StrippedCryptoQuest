using CryptoQuest.Battle.Components;
using CryptoQuest.Input;
using CryptoQuest.Menus.Status.States;
using CryptoQuest.Menus.Status.UI.Equipment;
using CryptoQuest.UI.Menu;
using CryptoQuest.UI.Tooltips.Events;
using FSM;
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
        [field: SerializeField] public UIEquipmentsInventory EquipmentsInventoryPanel { get; private set; }
        [field: SerializeField] public UICharacterStatsPanel CharacterStatsPanelPanel { get; private set; }
        [field: SerializeField] public ShowTooltipEvent ShowTooltipEvent { get; private set; }

        private StateMachine _stateMachine;

        public HeroBehaviour InspectingHero { get; set; }

        private void Awake() => _stateMachine = new StatusMenuStateMachine(this);

        private void OnEnable() => _stateMachine.Init();

        private void OnDisable()
        {
            ShowTooltipEvent.RaiseEvent(false);
            _stateMachine.OnExit();
        }
    }
}