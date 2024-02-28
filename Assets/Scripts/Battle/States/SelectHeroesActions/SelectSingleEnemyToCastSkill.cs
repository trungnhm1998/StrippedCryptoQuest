﻿using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.CommandDetail;
using CryptoQuest.Battle.UI.SelectSkill;
using UnityEngine.InputSystem;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    public class SelectSingleEnemyToCastSkill : StateBase
    {
        private readonly CastSkillAbility _selectedSkill;
        private readonly SelectSkillPresenter _skillPresenter;
        private readonly SelectEnemyPresenter _selectEnemyPresenter;

        public SelectSingleEnemyToCastSkill(CastSkillAbility selectedSkill, HeroBehaviour hero, SelectHeroesActions fsm) :
            base(hero, fsm)
        {
            Fsm.TryGetPresenterComponent(out _skillPresenter);
            _selectedSkill = selectedSkill;
            fsm.TryGetPresenterComponent<SelectEnemyPresenter>(out _selectEnemyPresenter);
        }

        public override void OnEnter()
        {
            _skillPresenter.Hide();
            _selectEnemyPresenter.Show();
            _selectEnemyPresenter.RegisterEnemySelectedCallback(CreateCommandToCastSkillOnEnemy);
        }

        public override void OnExit()
        {
            _selectEnemyPresenter.Hide();
        }

        public override void OnCancel() { }

        private void CreateCommandToCastSkillOnEnemy(EnemyBehaviour enemy)
        {
            var castSkillCommand =
                new CastSkillCommand(Hero, _selectedSkill, enemy);
            Hero.TryGetComponent(out CommandExecutor commandExecutor);
            commandExecutor.SetCommand(castSkillCommand);
            Fsm.GoToNextState();
        }
    }
}