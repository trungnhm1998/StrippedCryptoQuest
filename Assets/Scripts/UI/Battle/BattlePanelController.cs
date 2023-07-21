﻿using CryptoQuest.Gameplay.Battle.Core.Components;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using CryptoQuest.Input;
using CryptoQuest.UI.Battle.BattleActionHandler;
using CryptoQuest.UI.Battle.CommandsMenu;
using UnityEngine;
using UnityEngine.Events;
using NotImplementedException = System.NotImplementedException;

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

        [SerializeField] private NavigationAutoScroll _navigationAutoScroll;
        [SerializeField] private UIBattleCommandMenu _uiBattleCommandMenu;

        [SerializeField] private BattleActionHandler.BattleActionHandler[] _normalAttackChain;

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

            SetupChain(_normalAttackChain);
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

        private void SetupChain(BattleActionHandler.BattleActionHandler[] chain)
        {
            for (int i = 1; i < chain.Length; i++)
            {
                chain[i - 1].SetNext(chain[i]);
            }
        }

        private void OnButtonEscapeClickedHandler()
        {
            Debug.Log("Escape");
            _battleManager.OnEscape();
        }

        private void OnButtonGuardClickedHandler()
        {
            Debug.Log("Guard");
        }

        private void OnButtonItemClickedHandler()
        {
            Debug.Log("Item");
            OpenItemPanel();
        }

        private void OnButtonSkillClickedHandler()
        {
            Debug.Log("Skill");
            OpenSkillPanel();
        }

        private void OnButtonAttackClickedHandler(IBattleUnit currentUnit)
        {
            // _normalAttackChain[0].Handle(currentUnit);
            OpenAttackPanel();
        }

        private void OpenAttackPanel()
        {
            _attackPanel.content.SetActive(true);
            _skillPanel.content.SetActive(false);
            _itemPanel.content.SetActive(false);
            _mobPanel.content.SetActive(false);
        }

        private void OpenSkillPanel()
        {
            _attackPanel.content.SetActive(false);
            _skillPanel.content.SetActive(true);
            _itemPanel.content.SetActive(false);
            _mobPanel.content.SetActive(false);
        }

        private void OpenItemPanel()
        {
            _attackPanel.content.SetActive(false);
            _skillPanel.content.SetActive(false);
            _itemPanel.content.SetActive(true);
            _mobPanel.content.SetActive(false);
        }

        private void OpenMobPanel()
        {
            _attackPanel.content.SetActive(false);
            _skillPanel.content.SetActive(false);
            _itemPanel.content.SetActive(false);
            _mobPanel.content.SetActive(true);
        }
    }
}