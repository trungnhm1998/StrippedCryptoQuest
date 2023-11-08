using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI;
using CryptoQuest.Battle.UI.SelectSkill;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    public class SelectEnemyGroupToCastSkill : StateBase
    {
        private readonly CastSkillAbility _selectedSkill;
        private readonly SelectSkillPresenter _skillPresenter;
        private readonly EnemyGroupPresenter _enemyGroupPresenter;

        public SelectEnemyGroupToCastSkill(CastSkillAbility selectedSkill, HeroBehaviour hero,
            SelectHeroesActions fsm) : base(hero, fsm)
        {
            Fsm.TryGetComponent(out _skillPresenter);
            _selectedSkill = selectedSkill;
            _enemyGroupPresenter = fsm.BattleStateMachine.GetComponent<EnemyGroupPresenter>();
        }

        public override void OnEnter()
        {
            _skillPresenter.Hide();
            _enemyGroupPresenter.Show(true);
            _enemyGroupPresenter.RegisterSelectEnemyGroupCallback(CreateCommand);
        }

        public override void OnExit()
        {
            _enemyGroupPresenter.Hide();
        }

        public override void OnCancel() { }

        private void CreateCommand(EnemyGroup enemyGroup)
        {
            var castSkillCommand = new EnemyGroupCastSkillCommand(Hero, _selectedSkill, enemyGroup);
            Hero.TryGetComponent(out CommandExecutor commandExecutor);
            commandExecutor.SetCommand(castSkillCommand);
            Fsm.GoToNextState();
        }
    }
}