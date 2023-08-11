using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using CryptoQuest.Input;
using CryptoQuest.UI.Battle.CommandsMenu;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using UnityEngine.Events;
using CryptoQuest.UI.Battle.MenuStateMachine;

namespace CryptoQuest.UI.Battle
{
    public class BattlePanelController : MonoBehaviour
    {
        public UnityAction<IBattleUnit> OnButtonAttackClicked = delegate { };
        public UnityAction<IBattleUnit> OnButtonSkillClicked = delegate { };
        public UnityAction<IBattleUnit> OnButtonItemClicked = delegate { };
        public UnityAction OnButtonGuardClicked = delegate { };
        public UnityAction<IBattleUnit> OnButtonEscapeClicked = delegate { };


        [field: SerializeField] public InventorySO Inventory { get; private set; }
        [field: SerializeField] public CharacterList CharactersUI { get; private set; }

        [SerializeField] private BattleActionHandler.BattleActionHandler[] _normalAttackChain;
        [SerializeField] private BattleActionHandler.BattleActionHandler _retreatHandler;
        [SerializeField] private BattleActionHandler.BattleActionHandler[] _skillAttackChain;
        [SerializeField] private BattleActionHandler.BattleActionHandler[] _itemChain;

        [SerializeField] private BattleInputSO _battleInput;
        [SerializeField] private BattleBus _battleBus;

        [Header("Events")]
        [SerializeField] private VoidEventChannelSO _onNewTurnEvent;

        [Header("UI Panels")] [SerializeField] private UIBattleCommandMenu _uiBattleCommandMenu;

        [SerializeField] private UICommandPanel _commandPanel;

        public BattleManager BattleManager { get; private set; }

        public BattleMenuStateMachine BattleMenuFSM { get; private set; }

        public void OpenCommandDetailPanel(List<AbstractButtonInfo> infos)
        {
            _commandPanel.Clear();
            _commandPanel.gameObject.SetActive(true);
            _commandPanel.Init(infos);
        }

        public void CloseCommandDetailPanel()
        {
            _commandPanel.Clear();
            _commandPanel.gameObject.SetActive(false);
        }
        
        public void SetActiveCommandDetailButtons(bool isActive)
        {
            _commandPanel.SetActiveButtons(isActive);
        }

        private void Awake()
        {
            SetupChain(_normalAttackChain);
            SetupChain(_skillAttackChain);
            SetupChain(_itemChain);
        }

        private void OnEnable()
        {
            OnButtonAttackClicked += OnButtonAttackClickedHandler;
            OnButtonSkillClicked += OnButtonSkillClickedHandler;
            OnButtonItemClicked += OnButtonItemClickedHandler;
            OnButtonGuardClicked += OnButtonGuardClickedHandler;
            OnButtonEscapeClicked += OnButtonEscapeClickedHandler;

            _battleInput.CancelEvent += OnClickMenuCancel;

            BattleManager = _battleBus.BattleManager;
            SetupStateMachine();
        }

        private void OnDisable()
        {
            OnButtonAttackClicked -= OnButtonAttackClickedHandler;
            OnButtonSkillClicked -= OnButtonSkillClickedHandler;
            OnButtonItemClicked -= OnButtonItemClickedHandler;
            OnButtonGuardClicked -= OnButtonGuardClickedHandler;
            OnButtonEscapeClicked -= OnButtonEscapeClickedHandler;

            _battleInput.CancelEvent -= OnClickMenuCancel;
        }

        private void SetupChain(BattleActionHandler.BattleActionHandler[] chain)
        {
            for (var i = 1; i < chain.Length; i++)
            {
                chain[i - 1].SetNext(chain[i]);
            }
        }

        private void SetupStateMachine()
        {
            BattleMenuFSM?.ResetToNewState(BattleMenuStateMachine.SelectCommandState);

            BattleMenuFSM ??= new BattleMenuStateMachine(this);
        }

        public void ReinitializeUI()
        {
            _commandPanel.Clear();
            _uiBattleCommandMenu.Initialize();
        }

        public void SetActiveCommandMenu(bool value)
        {
            _uiBattleCommandMenu.SetActiveCommandsMenu(value);
        }

        private void OnClickMenuCancel()
        {
            BattleMenuFSM?.HandleCancel();
        }

        private void OnButtonEscapeClickedHandler(IBattleUnit currentUnit)
        {
            _commandPanel.Clear();
            _retreatHandler.CurrentBattleInfo = BattleManager.CurrentBattleInfo;
            _retreatHandler.Handle(currentUnit);
        }

        private void OnButtonGuardClickedHandler()
        {
            _commandPanel.Clear();
        }

        private void OnButtonItemClickedHandler(IBattleUnit currentUnit)
        {
            _commandPanel.Clear();
            _itemChain[0].Handle(currentUnit);
        }

        private void OnButtonSkillClickedHandler(IBattleUnit currentUnit)
        {
            _commandPanel.Clear();
            _skillAttackChain[0].Handle(currentUnit);
        }

        private void OnButtonAttackClickedHandler(IBattleUnit currentUnit)
        {
            currentUnit.SelectAbility(currentUnit.UnitLogic.NormalAttack);

            BattleMenuFSM.RequestStateChange(BattleMenuStateMachine.SelectSingleEnemyState);
        }
    }
}