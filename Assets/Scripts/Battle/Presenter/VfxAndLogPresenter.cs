using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.UI.Logs;
using CryptoQuest.Input;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;
using NotImplementedException = System.NotImplementedException;

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

        private void Awake()
        {
            Idle = new IdleState();
            Hide = new HideState();
            GetNextCommand = new GetNextCommandState();
            
            _startPresentingEvent = BattleEventBus.SubscribeEvent<StartPresentingEvent>(ShowDialog);

            _logPresenter.AppendingLogEvent += EnqueueLogNewCommand;
        }

        private void EnqueueLogNewCommand(LocalizedString message)
        {
            StartCoroutine(CoEnqueueLogNewCommand(message));
        }

        private IEnumerator CoEnqueueLogNewCommand(LocalizedString message)
        {
            var handle = message.GetLocalizedStringAsync();
            yield return handle;
            _commands.Enqueue(new PresentLogCommand(handle.Result));
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_startPresentingEvent);

            _logPresenter.AppendingLogEvent -= EnqueueLogNewCommand;

            _currentState?.Exit();
        }

        private void Start()
        {
            ChangeState(Hide);
        }

        private void ShowDialog(StartPresentingEvent startPresentingEvent)
        {
            ChangeState(Idle);
        }

        private void ClearDialog(TurnStartedEvent ctx)
        {
            _logPresenter.Clear();
        }

        private void HideAndClearLog(RoundEndedEvent ctx)
        {
            ChangeState(Hide);
        }

        public void ChangeState(StateBase state)
        {
            _currentState?.Exit();
            _currentState = state;
            _currentState.Enter(this, _logPresenter);
        }
    }
}