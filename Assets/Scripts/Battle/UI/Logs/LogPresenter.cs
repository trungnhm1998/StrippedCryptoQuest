using System;
using CryptoQuest.Battle.Presenter;
using CryptoQuest.UI.Dialogs.BattleDialog;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.UI.Logs
{
    /// <summary>
    /// This more like a view than a presenter because it not getting data from anywhere
    /// </summary>
    public class LogPresenter : MonoBehaviour
    {
        private const int MAX_LINE = 3;
        [SerializeField] private VfxAndLogPresenter _vfxAndLogPresenter;
        [SerializeField] private VoidEventChannelSO _sceneLoadedEvent;
        [SerializeField] private float _delayBetweenLines = 0.5f;
        public float DelayBetweenLines => _delayBetweenLines;
        [SerializeField] private float _hideDelay = 1f;
        private UIGenericDialog _dialog;

        private void Awake()
        {
            _sceneLoadedEvent.EventRaised += OnSceneLoaded; // Only start from editor need this
            var loggers = GetComponentsInChildren<LoggerComponentBase>();
            foreach (var logger in loggers)
            {
                logger.Init(this);
            }
        }

        private void OnDestroy()
        {
            _sceneLoadedEvent.EventRaised -= OnSceneLoaded;
            GenericDialogController.Instance.Release(_dialog);
        }

        private void OnSceneLoaded()
        {
            GenericDialogController.Instance.Instantiate(dialog => _dialog = dialog, false);
        }

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
            var command = new PresentLogCommand(message);
            _vfxAndLogPresenter.EnqueueCommand(command);
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