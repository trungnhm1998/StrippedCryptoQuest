using System;
using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
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

        public SelectingItem(HeroBehaviour hero, SelectHeroesActions fsm) : base(hero, fsm)
        {
            _selectItemPresenter = fsm.BattleStateMachine.gameObject.GetComponentInChildren<SelectItemPresenter>();
        }

        public override void OnEnter()
        {
            _selectItemPresenter.Show();
            
            _selectItemPresenter.SelectSingleEnemyCallback = SelectEnemyToUseItemOn;
            _selectItemPresenter.SelectSingleHeroCallback = SelectHeroToUseItemOn;
        }

        private void SelectEnemyToUseItemOn(UIItem itemUI)
        {
            Debug.Log("SelectingSkill::SelectEnemyToCastSkillOn");
            Fsm.PushState(new SelectSingleEnemyToUseItem(itemUI, Hero, Fsm));
        }

        private void SelectHeroToUseItemOn(UIItem itemUI)
        {
            Debug.Log("SelectingSkill::SelectHeroToCastSkillOn");
            Fsm.PushState(new SelectSingleHeroToUseItem(_selectItemPresenter, Hero, Fsm));
        }

        public override void OnExit() 
        {
            _selectItemPresenter.Hide();
        }

        public override void OnCancel() { }
    }
}