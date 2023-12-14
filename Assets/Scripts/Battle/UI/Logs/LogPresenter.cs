using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.Presenter;
using CryptoQuest.Battle.Presenter.Commands;
using CryptoQuest.UI.Dialogs.BattleDialog;
using IndiGames.Core.Events.ScriptableObjects;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace CryptoQuest.Battle.UI.Logs
{
    /// <summary>
    /// This more like a view than a presenter because it not getting data from anywhere
    /// </summary>
    public class LogPresenter : MonoBehaviour
    {
        private const int MAX_LINE = 3;

        [FormerlySerializedAs("_vfxAndLogPresenter")] [SerializeField]
        private RoundEventsPresenter _roundEventsPresenter;

        [SerializeField] private VoidEventChannelSO _sceneLoadedEvent;
        [SerializeField] private float _delayBetweenLines = 0.5f;
        public float DelayBetweenLines => _delayBetweenLines;
        [SerializeField] private float _hideDelay = 1f;
        private UIGenericDialog _dialog;
        private TinyMessageSubscriptionToken _turnStartingEvent;
        private TinyMessageSubscriptionToken _acceptCommandEvent;
        private TinyMessageSubscriptionToken _roundEndedEvent;

        private void Awake()
        {
            _acceptCommandEvent = BattleEventBus.SubscribeEvent<StartAcceptCommand>(QueueShowDialog);
            _roundEndedEvent = BattleEventBus.SubscribeEvent<RoundEndedEvent>(QueueHideDialog);
            _turnStartingEvent = BattleEventBus.SubscribeEvent<TurnStartedEvent>(QueueClearDialog);
            _sceneLoadedEvent.EventRaised += OnSceneLoaded; // Only start from editor need this
            var loggers = GetComponentsInChildren<LoggerComponentBase>();
            foreach (var logger in loggers)
            {
                logger.Init(this);
            }
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_acceptCommandEvent);
            BattleEventBus.UnsubscribeEvent(_roundEndedEvent);
            BattleEventBus.UnsubscribeEvent(_turnStartingEvent);
            _sceneLoadedEvent.EventRaised -= OnSceneLoaded;
            GenericDialogController.Instance.Release(_dialog);
        }

        private void QueueHideDialog(RoundEndedEvent _) => _roundEventsPresenter.EnqueueCommand(new HideDialog(this));

        private void QueueShowDialog(StartAcceptCommand ctx) =>
            _roundEventsPresenter.EnqueueCommand(new ShowDialog(this, ctx.RoundStartedContext.Round));

        private void QueueClearDialog(TurnStartedEvent ctx) => _roundEventsPresenter.EnqueueCommand(new ClearLog(this));

        private void OnSceneLoaded() => GenericDialogController.Instance.Instantiate(dialog => _dialog = dialog, false);

        public void Show()
        {
            if (_dialog) _dialog.Show();
        }

        public void QueueLog(LocalizedString message)
        {
            if (message.IsEmpty)
            {
                Debug.LogWarning("Try to append empty message");
                return;
            }

            Debug.Log($"LogPresenter::AppendLog {message.GetLocalizedString()}");
            _roundEventsPresenter.EnqueueCommand(new PresentLogCommand(this, message));
        }

        private int _lineCount;

        public void Append(string message)
        {
            if (_lineCount >= MAX_LINE)
            {
                _lineCount = 0;
                Clear();
            }

            _dialog.AppendMessage(message);
            _lineCount++;
        }

        public void Clear()
        {
            _lineCount = 0;
            if (_dialog) _dialog.Clear();
        }

        public void HideAndClear()
        {
            if (_dialog) _dialog.Hide();
            Clear();
        }
    }
}