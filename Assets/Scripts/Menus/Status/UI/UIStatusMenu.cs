using System;
using CryptoQuest.Battle.Components;
using CryptoQuest.Input;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Menus.Status.Events;
using CryptoQuest.Menus.Status.States;
using CryptoQuest.Menus.Status.UI.Equipment;
using CryptoQuest.Menus.Status.UI.MagicStone;
using CryptoQuest.UI.Menu;
using CryptoQuest.UI.Tooltips.Events;
using FSM;
using UnityEngine;
using State = CryptoQuest.Menus.Status.States.State;

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
        [field: SerializeField] public UICharacterStatsPanel CharacterStatsPanel { get; private set; }
        [field: SerializeField] public ShowTooltipEvent ShowTooltipEvent { get; private set; }
        [field: SerializeField] public ShowMagicStoneEvent ShowMagicStone { get; private set; }
        [field: SerializeField] public UIEquipmentDetails EquipmentDetails { get; private set; }
        [field: SerializeField] public GameObject MagicStoneMenu { get; private set; }
        [field: SerializeField] public UIAttachList UIAttachList { get; private set; }
        [field: SerializeField] public MagicStoneSelection MagicStoneSelection { get; private set; }
        [field: SerializeField] public StatusMenuEquipmentPreviewer EquipmentPreviewer { get; private set; }

        public ESlot ModifyingSlot { get; set; }
        public EEquipmentCategory ModifyingCategory { get; set; }

        private StateMachine _stateMachine;

        private HeroBehaviour _inspectingHero;
        private string _previousState;

        public HeroBehaviour InspectingHero
        {
            get => _inspectingHero;
            set
            {
                _inspectingHero = value;
                InspectingHeroChanged?.Invoke(_inspectingHero);
            }
        }

        public IEquipment InspectingEquipment { get; set; }

        public event Action<HeroBehaviour> InspectingHeroChanged;

        private void Awake() => _stateMachine = new StatusMenuStateMachine(this);

        private void OnEnable()
        {
            ShowMagicStone.EventRaised += ShowMagicStoneMenuRequested;
            _stateMachine.Init();
        }

        private void OnDisable()
        {
            ShowMagicStone.EventRaised -= ShowMagicStoneMenuRequested;
            ShowTooltipEvent.RaiseEvent(false);
            _stateMachine.OnExit();
            InspectingHero = null;
        }

        private void ShowMagicStoneMenuRequested(IEquipment equipment)
        {
            InspectingEquipment = equipment;
            ShowTooltipEvent.RaiseEvent(false);
            _previousState = _stateMachine.ActiveStateName;
            
            MagicStoneMenu.SetActive(true);
            EquipmentDetails.Init(InspectingEquipment);
            _stateMachine.RequestStateChange(State.MAGIC_STONE_SLOT_SELECTION);
        }

        public void BackToPreviousState()
        {
            if (string.IsNullOrEmpty(_previousState)) return;
            _stateMachine.RequestStateChange(_previousState);
            _previousState = "";
        }
    }
}