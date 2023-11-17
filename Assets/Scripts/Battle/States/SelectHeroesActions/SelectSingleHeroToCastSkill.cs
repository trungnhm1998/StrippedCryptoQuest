using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.SelectHero;
using CryptoQuest.Battle.UI.SelectSkill;
using UnityEngine.InputSystem;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    internal class SelectSingleHeroToCastSkill : StateBase
    {
        private readonly CastSkillAbility _selectedSkill;
        private readonly SelectHeroPresenter _selectHeroPresenter;
        private readonly SelectSkillPresenter _skillPresenter;

        public SelectSingleHeroToCastSkill(CastSkillAbility selectedSkill, HeroBehaviour hero, SelectHeroesActions fsm) :
            base(hero, fsm)
        {
            _selectedSkill = selectedSkill;
            Fsm.TryGetPresenterComponent(out _skillPresenter);
            Fsm.TryGetPresenterComponent(out _selectHeroPresenter);
        }

        public override void OnEnter()
        {
            _selectHeroPresenter.Show(_selectedSkill.SkillName);
            _skillPresenter.Show(Hero, false);
            _selectHeroPresenter.ConfirmSelectCharacter += CastSkillOnHero;
        }

        public override void OnExit()
        {
            _selectHeroPresenter.Hide();
            _skillPresenter.Hide();
            _selectHeroPresenter.ConfirmSelectCharacter -= CastSkillOnHero;
        }

        public override void OnCancel() { }

        private void CastSkillOnHero(HeroBehaviour selectedHero)
        {
            var castSkillCommand = new CastSkillCommand(Hero, _selectedSkill, selectedHero);
            Hero.TryGetComponent(out CommandExecutor commandExecutor);
            commandExecutor.SetCommand(castSkillCommand);
            Fsm.GoToNextState();
        }
    }
}