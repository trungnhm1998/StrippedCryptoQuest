using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.UI;
using CryptoQuest.Battle.UI.SelectCommand;
using CryptoQuest.Gameplay.Battle.Core.Helper;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    public class SelectCommand : StateBase, ISelectCommandCallback
    {
        private const float SELECT_DELAY = 0.1f;
        private EnemyGroupPresenter _enemyGroupPresenter;

        public SelectCommand(HeroBehaviour hero, SelectHeroesActions fsm) : base(hero, fsm)
        {
            Fsm.TryGetComponent(out _enemyGroupPresenter);
            _heroEventObject = new HighlightHeroEvent() { Hero = hero };
        }

        private GameObject _lastSelectedCommand;
        private HighlightHeroEvent _heroEventObject;

        public override void OnEnter()
        {
            BattleEventBus.RaiseEvent<HighlightHeroEvent>(_heroEventObject);
            
            Fsm.SelectCommandUI.SetCharacterName(Hero.Spec.Unit.Origin.DetailInformation.LocalizedName);
            Fsm.SelectCommandUI.RegisterCallback(this);
            
            DOVirtual.DelayedCall(SELECT_DELAY, SelectButton);

            _enemyGroupPresenter.Show();
            EnableCommandMenu();
        }

        public override void OnExit()
        {
            _enemyGroupPresenter.Hide();
            Fsm.SelectCommandUI.RegisterCallback(null);
            DisableCommandMenu();
        }

        public override void OnCancel()
        {
            Fsm.PopToLastSelectCommandState();
        }

        public void OnAttackPressed()
        {
            Debug.Log("SelectCommandState::OnAttackPressed");
            _lastSelectedCommand = EventSystem.current.currentSelectedGameObject;
            DisableCommandMenu();
            Fsm.PushState(new AttackEnemy(Hero, Fsm));
        }

        public void OnSkillPressed()
        {
            Debug.Log("SelectCommandState::OnSkillPressed");
            _lastSelectedCommand = EventSystem.current.currentSelectedGameObject;
            DisableCommandMenu();
            Fsm.PushState(new SelectingSkill(Hero, Fsm));
        }

        public void OnItemPressed()
        {
            Debug.Log("SelectCommandState::OnItemPressed");
            _lastSelectedCommand = EventSystem.current.currentSelectedGameObject;
            Fsm.PushState(new SelectingItem(Hero, Fsm));
        }

        public void OnGuardPressed()
        {
            Debug.Log("SelectCommandState::OnGuardPressed");
            _lastSelectedCommand = EventSystem.current.currentSelectedGameObject;
            Hero.TryGetComponent(out CommandExecutor commandExecutor);
            commandExecutor.SetCommand(new GuardCommand(Hero));
            Fsm.GoToNextState();
        }

        public void OnRetreatPressed()
        {
            Debug.Log("SelectCommandState::OnRetreatPressed");
            _lastSelectedCommand = EventSystem.current.currentSelectedGameObject;
            var highestAgi = Fsm.EnemyPartyManager.Enemies
                .GetHighestAttributeValue<EnemyBehaviour>(AttributeSets.Agility);
            Hero.TryGetComponent(out CommandExecutor commandExecutor);
            commandExecutor.SetCommand(new RetreatCommand(Hero, highestAgi));

            Fsm.GoToNextState();
        }

        private void EnableCommandMenu()
        {
            Fsm.SelectCommandUI.SetActiveCommandsMenu(true);
        }

        private void DisableCommandMenu()
        {
            Fsm.SelectCommandUI.SetActiveCommandsMenu(false);
        }

        private void SelectButton()
        {
            if (_lastSelectedCommand != null)
                EventSystem.current.SetSelectedGameObject(_lastSelectedCommand);
            else
                Fsm.SelectCommandUI.SelectFirstButton();
        }
    }
}