using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Events.Gameplay;
using CryptoQuest.Gameplay.Battle.Core.Components;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills;
using CryptoQuest.Input;
using CryptoQuest.UI.Battle.CommandsMenu;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Battle
{
    public class BattlePanelController : MonoBehaviour
    {
        public UnityAction<IBattleUnit> OnButtonAttackClicked = delegate { };
        public UnityAction<IBattleUnit> OnButtonSkillClicked = delegate { };
        public UnityAction<IBattleUnit> OnButtonItemClicked = delegate { };
        public UnityAction OnButtonGuardClicked = delegate { };
        public UnityAction<IBattleUnit> OnButtonEscapeClicked = delegate { };


        [SerializeField] private BattleActionHandler.BattleActionHandler[] _normalAttackChain;
        [SerializeField] private BattleActionHandler.BattleActionHandler _retreatHandler;
        [SerializeField] private BattleActionHandler.BattleActionHandler[] _skillAttackChain;

        [Header("Events")] [SerializeField] private InputMediatorSO _inputMediator;

        [SerializeField] private BattleBus _battleBus;
        [SerializeField] private VoidEventChannelSO _onNewTurnEvent;

        [Header("UI Panels")] [SerializeField] private UIBattleCommandMenu _uiBattleCommandMenu;

        [SerializeField] private UICommandPanel _commandPanel;

        private BattleManager _battleManager;
        private List<AbstractButtonInfo> _infos = new();

        public void OpenCommandDetailPanel(List<AbstractButtonInfo> infos)
        {
            _commandPanel.gameObject.SetActive(true);
            _commandPanel.Init(infos);
        }

        public void CloseCommandDetailPanel()
        {
            _commandPanel.Clear();
            _commandPanel.gameObject.SetActive(false);
        }

        private void Awake()
        {
            SetupChain(_normalAttackChain);
            SetupChain(_skillAttackChain);
        }

        private void OnEnable()
        {
            OnButtonAttackClicked += OnButtonAttackClickedHandler;
            OnButtonSkillClicked += OnButtonSkillClickedHandler;
            OnButtonItemClicked += OnButtonItemClickedHandler;
            OnButtonGuardClicked += OnButtonGuardClickedHandler;
            OnButtonEscapeClicked += OnButtonEscapeClickedHandler;

            _inputMediator.CancelEvent += OnClickCancel;
            _battleManager = _battleBus.BattleManager;
            ShowEnemyGroups();
        }

        private void OnDisable()
        {
            OnButtonAttackClicked -= OnButtonAttackClickedHandler;
            OnButtonSkillClicked -= OnButtonSkillClickedHandler;
            OnButtonItemClicked -= OnButtonItemClickedHandler;
            OnButtonGuardClicked -= OnButtonGuardClickedHandler;
            OnButtonEscapeClicked -= OnButtonEscapeClickedHandler;

            _inputMediator.CancelEvent -= OnClickCancel;
        }

        private void SetupChain(BattleActionHandler.BattleActionHandler[] chain)
        {
            for (int i = 1; i < chain.Length; i++)
            {
                chain[i - 1].SetNext(chain[i]);
            }
        }

        private void ShowEnemyGroups()
        {
            _commandPanel.Clear();
            _infos.Clear();
            var opponentTeam = _battleManager.BattleTeam2;
            
            foreach (var group in opponentTeam.TeamGroups.GroupsDict)
            {
                _infos.Add(new EnemyGroupButtonInfo(group.Key, group.Value));
            }

            OpenCommandDetailPanel(_infos);
        }

        public void ReinitializeUI()
        {
            _commandPanel.Clear();
            _uiBattleCommandMenu.Initialize();
        }

        private void OnClickCancel()
        {
            ReinitializeUI();
        }

        private void OnButtonEscapeClickedHandler(IBattleUnit currentUnit)
        {
            _commandPanel.Clear();
            _retreatHandler.CurrentBattleInfo = _battleManager.CurrentBattleInfo;
            _retreatHandler.Handle(currentUnit);
        }

        private void OnButtonGuardClickedHandler()
        {
            _commandPanel.Clear();
        }

        private void OnButtonItemClickedHandler(IBattleUnit currentUnit) { }

        private void OnButtonSkillClickedHandler(IBattleUnit currentUnit)
        {
            _commandPanel.Clear();
            _skillAttackChain[0].Handle(currentUnit);
        }

        private void OnButtonAttackClickedHandler(IBattleUnit currentUnit)
        {
            _commandPanel.Clear();
            _normalAttackChain[0].Handle(currentUnit);
        }
    }
}