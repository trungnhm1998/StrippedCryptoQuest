using System.Collections.Generic;
using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.UI.Logs;
using CryptoQuest.Input;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle.Presenter
{
    public class VfxAndLogPresenter : MonoBehaviour
    {
        [SerializeField] private BattleInput _input;
        public BattleInput Input => _input;
        [SerializeField] private LogPresenter _logPresenter;

        private readonly Queue<IPresentCommand> _commands = new();
        public Queue<IPresentCommand> Commands => _commands;

        private StateBase _currentState;
        public StateBase Idle { get; private set; }
        public StateBase Hide { get; private set; }
        public StateBase GetNextCommand { get; private set; }

        private TinyMessageSubscriptionToken _startPresentingEvent;
        private TinyMessageSubscriptionToken _turnStartingEvent;

        private void Awake()
        {
            Idle = new IdleState();
            Hide = new HideState();
            GetNextCommand = new GetNextCommandState();

            _startPresentingEvent = BattleEventBus.SubscribeEvent<StartPresentingEvent>(ShowDialog);
            _turnStartingEvent = BattleEventBus.SubscribeEvent<TurnStartedEvent>(ClearDialog);
        }

        public void EnqueueCommand(IPresentCommand command) => _commands.Enqueue(command);

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_startPresentingEvent);
            BattleEventBus.UnsubscribeEvent(_turnStartingEvent);

            _currentState?.Exit();
        }

        private void Start() => ChangeState(Hide);

        private void ShowDialog(StartPresentingEvent startPresentingEvent) => ChangeState(Idle);

        private void ClearDialog(TurnStartedEvent ctx) => _commands.Enqueue(new ClearLog(_logPresenter));

        public void ChangeState(StateBase state)
        {
            _currentState?.Exit();
            _currentState = state;
            _currentState.Enter(this, _logPresenter);
        }
    }
}