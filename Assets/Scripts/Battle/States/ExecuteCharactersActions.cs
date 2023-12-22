using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.Presenter.Commands;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle.States
{
    /// <summary>
    /// This also update characters effects/buffs and debuffs on their turns
    /// </summary>
    public class ExecuteCharactersActions : IState
    {
        private BattlePresenter _presenter;
        private BattleContext _battleContext;
        private BattleStateMachine _stateMachine;

        private bool _isBattleEnded;
        private int _roundCount;

        private TinyMessageSubscriptionToken _retreatSucceedEvent;

        private readonly RoundEndedEvent _roundEndedEvent = new();

        public void OnEnter(BattleStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _battleContext = stateMachine.GetComponent<BattleContext>();
            _presenter = stateMachine.BattlePresenter;
            _presenter.CommandPanel.SetActive(false);

            _retreatSucceedEvent = BattleEventBus.SubscribeEvent<RetreatSucceedEvent>(OnRetreatSuccess);

            ExecuteCharactersCommands();
        }

        public void OnExit(BattleStateMachine stateMachine)
        {
            BattleEventBus.UnsubscribeEvent(_retreatSucceedEvent);
        }

        private void ExecuteCharactersCommands()
        {
            BattleEventBus.RaiseEvent(new RoundStartedEvent { Round = ++_roundCount });
            var characters = _battleContext.GetSortedAliveCharacterBasedOnAgi();
            Debug.Log($"Battle::Execute actions for round [{_roundCount}]");

            foreach (var character in characters)
            {
                if (character.IsValidAndAlive() == false) continue; // could die because of last turn
                OnTurnStarting(character);
                character.OnTurnStarted();
                character.TryGetComponent(out CommandExecutor commandExecutor);
                OnExecutingCommand(character);
                commandExecutor.ExecuteCommand();
                character.OnTurnEnded();
                OnTurnEnded(character);
                if (IsRoundContinuable() == false) break;
            }

            BattleEventBus.RaiseEvent(_roundEndedEvent); // Need to be raise so guard tag can be remove
            OnRoundHaveResult();

            // TODO: Server will change to wait for input/select command state instead
            _stateMachine.ChangeState(new PresentActions());
        }

        private static void OnTurnEnded(Components.Character character) => 
            BattleEventBus.RaiseEvent(new TurnEndedEvent(character));

        private static void OnTurnStarting(Components.Character character) =>
            BattleEventBus.RaiseEvent(new TurnStartedEvent(character));

        private static void OnExecutingCommand(Components.Character character) =>
            BattleEventBus.RaiseEvent(new ExecutingCommandEvent(character));

        private bool IsRoundContinuable()
        {
            _battleContext.UpdateBattleContext();
            return !IsLost() && !IsWon() && !_isBattleEnded;
        }

        /// <summary>
        /// Will raise lost or won event
        /// 
        /// Need at least one surviving character to win
        /// </summary>
        private void OnRoundHaveResult()
        {
            if (IsLost())
            {
                BattleEventBus.RaiseEvent(new TurnLostEvent());
                return;
            }

            if (IsWon())
            {
                BattleEventBus.RaiseEvent(new TurnWonEvent());
                return;
            }
        }

        private bool IsWon() => _battleContext.IsAllEnemiesDead;

        private bool IsLost() => _battleContext.IsAllHeroesDead;

        /// <summary>
        /// When battle ended with retreat player will not have reward 
        /// but still stay in the battle field scene
        /// </summary>
        public void OnEndBattle()
        {
            BattleEventBus.RaiseEvent(new RetreatedEvent());
        }

        private void OnRetreatSuccess(RetreatSucceedEvent retreatSucceedEvent)
        {
            _isBattleEnded = true;
            var presentCommand = new RetreatSuccessCommand(this);
            BattleEventBus.RaiseEvent(new EnqueuePresentCommandEvent(presentCommand));
        }
    }
}