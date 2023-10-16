using System;
using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.UI.CommandDetail;
using CryptoQuest.Battle.UI.SelectCommand;
using CryptoQuest.Battle.UI.SelectItem;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.Helper;
using CryptoQuest.Item;
using CryptoQuest.System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    public class SelectingItem : StateBase
    {
        private SelectItemPresenter _selectItemPresenter;
        private ConsumableInfo _selectedItem;

        public SelectingItem(HeroBehaviour hero, SelectHeroesActions fsm) : base(hero, fsm)
        {
            _selectItemPresenter = fsm.BattleStateMachine.gameObject.GetComponentInChildren<SelectItemPresenter>();
        }

        public override void OnEnter()
        {
            _selectItemPresenter.Show();
            _selectItemPresenter.SelectedItemEvent += CacheSelectedItem;

            _selectItemPresenter.SelectSingleEnemyCallback = SelectEnemyToUseItemOn;
            _selectItemPresenter.SelectSingleHeroCallback = SelectHeroToUseItemOn;
            _selectItemPresenter.SelectAllEnemyCallback = SelectAllEnemyToUseItemOn;
            _selectItemPresenter.SelectAllHeroCallback = SelectAllHeroToUseItemOn;
        }

        private void CacheSelectedItem(ConsumableInfo item)
        {
            // I cached instead of using SelectedItem in Presenter
            // because SelectedItem in Presenter is mutual between hero select state
            _selectedItem = item;
        }

        private void SelectEnemyToUseItemOn(ConsumableInfo item)
        {
            Debug.Log("SelectingSkill::SelectEnemyToCastSkillOn");
            Fsm.PushState(new SelectSingleEnemyToUseItem(item, Hero, Fsm));
        }

        private void SelectHeroToUseItemOn(ConsumableInfo item)
        {
            Debug.Log("SelectingSkill::SelectHeroToCastSkillOn");
            Fsm.PushState(new SelectSingleHeroToUseItem(_selectItemPresenter, Hero, Fsm));
        }

        private void SelectAllEnemyToUseItemOn(ConsumableInfo item)
        {
            Debug.Log("SelectingSkill::SelectAllEnemyToUseItemOn");

            var enemies = Fsm.EnemyPartyManager.Enemies.ToArray();
            CreateMultipleTargetCommand(enemies);
        }

        private void SelectAllHeroToUseItemOn(ConsumableInfo item)
        {
            Debug.Log("SelectingSkill::SelectAllHeroToUseItemOn");

            var heroes = Fsm.PlayerParty.OrderedAliveMembers.ToArray();
            CreateMultipleTargetCommand(heroes);
        }

        private void CreateMultipleTargetCommand(params Components.Character[] characters)
        {
            var useItemCommand = new ConsumeItemCommand(Hero, _selectItemPresenter.SelectedItem, characters);
            Hero.TryGetComponent(out CommandExecutor commandExecutor);
            commandExecutor.SetCommand(useItemCommand);
            Fsm.GoToNextState();
        }

        public override void OnExit() 
        {
            _selectItemPresenter.SelectedItemEvent -= CacheSelectedItem;
            _selectItemPresenter.Hide();
        }

        public override void OnCancel() 
        {
            // Only raise event if hero select item this state
            if (_selectedItem == null || !_selectedItem.IsValid()) return;
            BattleEventBus.RaiseEvent<CancelSelectedItemEvent>(
                new CancelSelectedItemEvent() 
                { 
                    ItemInfo = _selectedItem
                }
            );
        }
    }
}