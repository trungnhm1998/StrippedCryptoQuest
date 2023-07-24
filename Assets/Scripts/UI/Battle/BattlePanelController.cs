using CryptoQuest.Gameplay.Battle.Core.Components;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using CryptoQuest.Input;
using CryptoQuest.UI.Battle.CommandsMenu;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Battle
{
    public class BattlePanelController : MonoBehaviour
    {
        public UnityAction<IBattleUnit> OnButtonAttackClicked = delegate { };
        public UnityAction OnButtonSkillClicked = delegate { };
        public UnityAction OnButtonItemClicked = delegate { };
        public UnityAction OnButtonGuardClicked = delegate { };
        public UnityAction OnButtonEscapeClicked = delegate { };

        [Header("Events")]
        [SerializeField] private InputMediatorSO _inputMediator;

        [SerializeField] private BattleBus _battleBus;

        [SerializeField] private NavigationAutoScroll _navigationAutoScroll;
        [SerializeField] private UIBattleCommandMenu _uiBattleCommandMenu;

        [Header("UI Panels")]
        [SerializeField] private UIAttackPanel _attackPanel;

        [SerializeField] private UISkillsPanel _skillPanel;
        [SerializeField] private UIItemsPanel _itemPanel;
        [SerializeField] private UIMobStatusPanel _mobPanel;

        private BattleManager _battleManager;

        private void OnEnable()
        {
            AbstractBattlePanelContent[] panels = { _attackPanel, _skillPanel, _itemPanel, _mobPanel };

            foreach (var panel in panels)
            {
                panel.Init();
            }

            OnButtonAttackClicked += OnButtonAttackClickedHandler;
            OnButtonSkillClicked += OnButtonSkillClickedHandler;
            OnButtonItemClicked += OnButtonItemClickedHandler;
            OnButtonGuardClicked += OnButtonGuardClickedHandler;
            OnButtonEscapeClicked += OnButtonEscapeClickedHandler;

            _inputMediator.MenuNavigateEvent += OnChangeLine;
            _inputMediator.CancelEvent += OnClickCancel;

            _battleManager = _battleBus.BattleManager;

            OpenMobPanel();
        }

        private void OnDisable()
        {
            OnButtonAttackClicked -= OnButtonAttackClickedHandler;
            OnButtonSkillClicked -= OnButtonSkillClickedHandler;
            OnButtonItemClicked -= OnButtonItemClickedHandler;
            OnButtonGuardClicked -= OnButtonGuardClickedHandler;
            OnButtonEscapeClicked -= OnButtonEscapeClickedHandler;

            _inputMediator.MenuNavigateEvent -= OnChangeLine;
            _inputMediator.CancelEvent -= OnClickCancel;
        }


        private void OnChangeLine()
        {
            _navigationAutoScroll.CheckButtonPosition();
        }


        private void OnClickCancel()
        {
            OpenMobPanel();
            _uiBattleCommandMenu.Initialize();
        }

        private void OnButtonEscapeClickedHandler()
        {
            _battleManager.OnEscape();
        }

        private void OnButtonGuardClickedHandler() { }

        private void OnButtonItemClickedHandler()
        {
            OpenItemPanel();
        }

        private void OnButtonSkillClickedHandler()
        {
            OpenSkillPanel();
        }

        private void OnButtonAttackClickedHandler(IBattleUnit currentUnit)
        {
            OpenAttackPanel();
        }

        private void OpenAttackPanel()
        {
            _attackPanel.SetPanelActive(true);
            _skillPanel.SetPanelActive(false);
            _itemPanel.SetPanelActive(false);
            _mobPanel.SetPanelActive(false);
        }

        private void OpenSkillPanel()
        {
            _attackPanel.SetPanelActive(false);
            _skillPanel.SetPanelActive(true);
            _itemPanel.SetPanelActive(false);
            _mobPanel.SetPanelActive(false);
        }

        private void OpenItemPanel()
        {
            _attackPanel.SetPanelActive(false);
            _skillPanel.SetPanelActive(false);
            _itemPanel.SetPanelActive(true);
            _mobPanel.SetPanelActive(false);
        }

        private void OpenMobPanel()
        {
            _attackPanel.SetPanelActive(false);
            _skillPanel.SetPanelActive(false);
            _itemPanel.SetPanelActive(false);
            _mobPanel.SetPanelActive(true);
        }
    }
}