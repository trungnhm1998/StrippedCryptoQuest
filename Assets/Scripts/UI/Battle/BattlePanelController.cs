using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using CryptoQuest.Input;
using CryptoQuest.UI.Battle.CommandsMenu;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle
{
    public class BattlePanelController : MonoBehaviour
    {
        public UnityAction<IBattleUnit> OnButtonAttackClicked = delegate { };
        public UnityAction<IBattleUnit> OnButtonSkillClicked = delegate { };
        public UnityAction<IBattleUnit> OnButtonItemClicked = delegate { };
        public UnityAction OnButtonGuardClicked = delegate { };
        public UnityAction OnButtonEscapeClicked = delegate { };

        [Header("Events")]
        [SerializeField] private InputMediatorSO _inputMediator;

        [SerializeField] private BattleBus _battleBus;

        [SerializeField] private NavigationAutoScroll _navigationAutoScroll;
        [SerializeField] private UIBattleCommandMenu _uiBattleCommandMenu;

        [Header("UI Panels")]
        [SerializeField] private UICommandPanel _commandPanel;

        [Header("Demo Panels")]
        [SerializeField] private List<ButtonInfo> _attackPanelInfo;

        [SerializeField] private List<ButtonInfo> _skillPanelInfo;
        [SerializeField] private List<ButtonInfo> _itemPanelInfo;
        [SerializeField] private List<ButtonInfo> _mobInfo;

        private BattleManager _battleManager;


        private void OnEnable()
        {
            OnButtonAttackClicked += OnButtonAttackClickedHandler;
            OnButtonSkillClicked += OnButtonSkillClickedHandler;
            OnButtonItemClicked += OnButtonItemClickedHandler;
            OnButtonGuardClicked += OnButtonGuardClickedHandler;
            OnButtonEscapeClicked += OnButtonEscapeClickedHandler;

            _inputMediator.MenuNavigateEvent += OnChangeLine;
            _inputMediator.CancelEvent += OnClickCancel;

            _battleManager = _battleBus.BattleManager;

            _commandPanel.Init(_mobInfo);
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
            _commandPanel.Clear();
            _commandPanel.Init(_mobInfo);
            _uiBattleCommandMenu.Initialize();
            OnChangeLine();
        }

        private void OnButtonEscapeClickedHandler()
        {
            _commandPanel.Clear();
            _battleManager.OnEscape();
        }

        private void OnButtonGuardClickedHandler()
        {
            _commandPanel.Clear();
        }

        private void OnButtonItemClickedHandler(IBattleUnit currentUnit)
        {
            _commandPanel.Clear();
            _commandPanel.Init(_itemPanelInfo);
        }

        private void OnButtonSkillClickedHandler(IBattleUnit currentUnit)
        {
            _commandPanel.Clear();
            _commandPanel.Init(_skillPanelInfo);
        }

        private void OnButtonAttackClickedHandler(IBattleUnit currentUnit)
        {
            _commandPanel.Clear();
            _commandPanel.Init(_attackPanelInfo);
        }
    }
}