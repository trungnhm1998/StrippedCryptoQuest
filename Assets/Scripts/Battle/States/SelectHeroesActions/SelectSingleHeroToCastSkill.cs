using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.SelectHero;
using CryptoQuest.Battle.UI.SelectSkill;
using UnityEngine.InputSystem;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    internal class SelectSingleHeroToCastSkill : StateBase
    {
        private readonly UISkill _selectedSkillUI;
        private readonly SelectHeroPresenter _selectHeroPresenter;
        private readonly SelectSkillPresenter _skillPresenter;

        public SelectSingleHeroToCastSkill(UISkill selectedSkillUI, HeroBehaviour hero, SelectHeroesActions fsm) :
            base(hero, fsm)
        {
            _selectedSkillUI = selectedSkillUI;
            _selectHeroPresenter = Fsm.BattleStateMachine.SelectHeroPresenter;
            Fsm.TryGetComponent(out _skillPresenter);
        }

        public override void OnEnter()
        {
            _selectHeroPresenter.Show(_selectedSkillUI.Skill.Parameters.SkillName);
            _skillPresenter.Show(Hero, false);
            SelectHeroPresenter.ConfirmSelectCharacter += CastSkillOnHero;
        }

        public override void OnExit()
        {
            _selectHeroPresenter.Hide();
            _skillPresenter.Hide();
            SelectHeroPresenter.ConfirmSelectCharacter -= CastSkillOnHero;
        }

        public override void OnCancel() { }

        private void CastSkillOnHero(HeroBehaviour selectedHero)
        {
            var castSkillCommand = new CastSkillCommand(Hero, _selectedSkillUI.Skill, selectedHero);
            Hero.SetCommand(castSkillCommand);
            Fsm.GoToNextState();
        }
    }
}