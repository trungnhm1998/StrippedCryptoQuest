using CryptoQuest.Events.UI.Menu;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Input;
using CryptoQuest.Inventory;
using CryptoQuest.Menus.Home.States;
using CryptoQuest.Menus.Home.UI.CharacterList;
using CryptoQuest.UI.Menu;
using FSM;
using UnityEngine;

namespace CryptoQuest.Menus.Home.UI
{
    public class UIHomeMenu : UIMenuPanelBase
    {
        [field: Header("Events")]
        [field: SerializeField] public HeroInventoryFilledEvent InventoryFilled { get; private set; }

        [Header("State Context")]
        [SerializeField] private UIHomeMenuSortCharacter _sortMode;
        [field: SerializeField] public InputMediatorSO Input { get; private set; }
        [field: SerializeField] public UIOverview UIOverview { get; private set; }
        [field: SerializeField] public PartySO PartySO { get; private set; }

        [field: Header("Character List")]
        [field: SerializeField] public UICharacterList UICharacterList { get; private set; }
        [field: SerializeField] public UICharacterDetails UICharacterDetails { get; private set; }
        [field: SerializeField] public HeroSpecInitializer HeroSpecInitializer { get; private set; }

        public UIHomeMenuSortCharacter SortMode => _sortMode;

        private StateMachine _stateMachine;

        private void Awake()
        {
            _stateMachine = new HomeMenuStateMachine(this);
        }

        private void OnEnable()
        {
            _stateMachine.Init();
        }

        private void OnDisable()
        {
            _stateMachine.OnExit();
        }
    }
}