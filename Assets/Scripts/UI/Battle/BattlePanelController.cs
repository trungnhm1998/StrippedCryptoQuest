using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using CryptoQuest.Input;
using CryptoQuest.UI.Battle.CommandsMenu;
using IndiGames.Core.Events.ScriptableObjects;
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


        [SerializeField] private BattleActionHandler.BattleActionHandler[] _normalAttackChain;

        [Header("Events")]
        [SerializeField] private InputMediatorSO _inputMediator;

        [SerializeField] private BattleBus _battleBus;
        [SerializeField] private VoidEventChannelSO _onNewTurnEvent;

        [Header("UI Panels")]
        [SerializeField] private UIBattleCommandMenu _uiBattleCommandMenu;

        [SerializeField] private UICommandPanel _commandPanel;

        private BattleManager _battleManager;

        public void OpenCommandDetailPanel(List<ButtonInfo> infos)
        {
            _commandPanel.gameObject.SetActive(true);
            _commandPanel.Init(infos);
        }

        public void CloseCommandDetailPanel()
        {
            _commandPanel.Clear();
            _commandPanel.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            OnButtonAttackClicked += OnButtonAttackClickedHandler;
            OnButtonSkillClicked += OnButtonSkillClickedHandler;
            OnButtonItemClicked += OnButtonItemClickedHandler;
            OnButtonGuardClicked += OnButtonGuardClickedHandler;
            OnButtonEscapeClicked += OnButtonEscapeClickedHandler;
            _onNewTurnEvent.EventRaised += OnNewTurn;

            _inputMediator.CancelEvent += OnClickCancel;

            _battleManager = _battleBus.BattleManager;
            SetupChain(_normalAttackChain);
        }

        private void OnDisable()
        {
            OnButtonAttackClicked -= OnButtonAttackClickedHandler;
            OnButtonSkillClicked -= OnButtonSkillClickedHandler;
            OnButtonItemClicked -= OnButtonItemClickedHandler;
            OnButtonGuardClicked -= OnButtonGuardClickedHandler;
            OnButtonEscapeClicked -= OnButtonEscapeClickedHandler;
            _onNewTurnEvent.EventRaised -= OnNewTurn;

            _inputMediator.CancelEvent -= OnClickCancel;
        }

        private void SetupChain(BattleActionHandler.BattleActionHandler[] chain)
        {
            for (int i = 1; i < chain.Length; i++)
            {
                chain[i - 1].SetNext(chain[i]);
            }
        }

        private void OnNewTurn()
        {
            _commandPanel.Clear();
            List<ButtonInfo> infos = new();
            foreach (var enemy in _battleManager.BattleTeam2.BattleUnits)
            {
                infos.Add(new ButtonInfo()
                {
                    Name = enemy.UnitData.DisplayName,
                });
            }

            // OpenCommandDetailPanel(infos);
        }

        private void OnClickCancel()
        {
            _commandPanel.Clear();
            _uiBattleCommandMenu.Initialize();
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
        }

        private void OnButtonSkillClickedHandler(IBattleUnit currentUnit)
        {
            _commandPanel.Clear();
        }

        private void OnButtonAttackClickedHandler(IBattleUnit currentUnit)
        {
            _commandPanel.Clear();
            _normalAttackChain[0].Handle(currentUnit);
        }
    }
}