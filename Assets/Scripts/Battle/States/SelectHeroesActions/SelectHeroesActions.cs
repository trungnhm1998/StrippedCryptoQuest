using System.Collections.Generic;
using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.SelectCommand;
using CryptoQuest.Character.Tag;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    public abstract class StateBase
    {
        protected StateBase(SelectHeroesActions fsm) => Fsm = fsm;
        protected SelectHeroesActions Fsm { get; private set; }

        public abstract void OnEnter();
        public abstract void OnExit();
    }

    public class StateFactory
    {
        public static StateBase CreateState<T>() where T : StateBase, new()
        {
            return new T();
        }
    }

    /// <summary>
    /// Select the command of all the heroes in party
    ///
    /// only exit this state after selected commands for all heroes
    ///
    /// This class has a Push down state machine for each member/hero in party
    /// </summary>
    public class SelectHeroesActions : IState
    {
        public struct HeroCommand
        {
            public HeroBehaviour HeroBehaviour;
            public ICommand Command;
        }

        private UISelectCommand _selectCommandUI;
        public UISelectCommand SelectCommandUI => _selectCommandUI;
        private BattleStateMachine _battleStateMachine;
        public BattleStateMachine BattleStateMachine => _battleStateMachine;
        private IPartyController _party;
        private EnemyPartyManager _enemyPartyManager;
        public EnemyPartyManager EnemyPartyManager => _enemyPartyManager;
        private int _currentHeroIndex = 0;
        public StateFactory StateFactory { get; private set; } = new();
        private Stack<HeroCommand> HeroCommands { get; set; } = new Stack<HeroCommand>();

        private readonly Stack<StateBase> _stateStack = new Stack<StateBase>();

        public void OnEnter(BattleStateMachine battleStateMachine)
        {
            _battleStateMachine = battleStateMachine;
            _enemyPartyManager = battleStateMachine.GetComponent<EnemyPartyManager>();
            _selectCommandUI = battleStateMachine.CommandUI;
            _party = ServiceProvider.GetService<IPartyController>();

            _currentHeroIndex = 0;

            battleStateMachine.BattleUI.SetActive(true);
            PushState(new SelectCommand(_party.Slots[_currentHeroIndex].HeroBehaviour, this));
        }

        public void OnExit(BattleStateMachine battleStateMachine)
        {
            // battleStateMachine.BattleUI.SetActive(false);
        }

        public void PushState(StateBase state)
        {
            if (_stateStack.Count > 0) _stateStack.Peek()?.OnExit();
            _stateStack.Push(state);
            state.OnEnter();
        }

        public void PopState()
        {
            _stateStack.Peek()?.OnExit();
            _stateStack.Pop();
            if (_stateStack.Count > 0)
                _stateStack.Peek()?.OnEnter();
        }

        public bool GetNextAliveHero(out HeroBehaviour hero)
        {
            hero = null;
            if (++_currentHeroIndex>= _party.Size) return false;
            bool canHeroFunctions;
            do
            {
                hero = _party.Slots[_currentHeroIndex].HeroBehaviour;
                canHeroFunctions = hero != null && hero.IsValid() && !hero.HasTag(TagsDef.Dead);
            } while (canHeroFunctions == false);

            return true;
        }

        public void AddCommand(HeroBehaviour heroBehaviour, ICommand command)
        {
            HeroCommands.Push(new HeroCommand
            {
                HeroBehaviour = heroBehaviour,
                Command = command
            });
        }

        public void GoToPresentState()
        {
            _battleStateMachine.ChangeState(new Present(HeroCommands));
        }
    }
}