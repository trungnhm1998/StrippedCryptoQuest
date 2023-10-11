using System.Collections.Generic;
using CryptoQuest.UI.Dialogs.BattleDialog;
using FSM;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.UI.Logs
{
    public class LogPresenter : MonoBehaviour
    {
        private const int MAX_LINE = 3;
        [SerializeField] private VoidEventChannelSO _sceneLoadedEvent;
        [SerializeField] private float _delayBetweenLines = 0.5f;
        [SerializeField] private float _hideDelay = 1f;
        private UIGenericDialog _dialog;
        private readonly Queue<LocalizedString> _lines = new();
        public bool Finished => _presenting == false && _lines.Count == 0;
        private int _count;
        private bool _presenting;
        private float _timer;

        private void Awake()
        {
            _sceneLoadedEvent.EventRaised += OnSceneLoaded; // Only start from editor need this
            var loggers = GetComponentsInChildren<LoggerComponentBase>();
            foreach (var logger in loggers)
            {
                logger.Init(this);
            }
        }

        private void OnDisable()
        {
            _sceneLoadedEvent.EventRaised -= OnSceneLoaded;
            GenericDialogController.Instance.Release(_dialog);
        }

        private void OnSceneLoaded()
        {
            GenericDialogController.Instance.Instantiate(dialog => _dialog = dialog, false);
        }

        public void Show() => _dialog.Show();

        public void AppendLog(LocalizedString message)
        {
            if (message.IsEmpty)
            {
                Debug.LogWarning("Try to append empty message");
                return;
            }

            Debug.Log($"LogPresenter::AppendLog {message.GetLocalizedString()}");
            _lines.Enqueue(message);
        }

        public void Clear()
        {
            _presenting = false;
            _count = MAX_LINE;
            _lines.Clear();
            _dialog.Clear();
        }

        public void HideAndClear()
        {
            _dialog.Hide();
            Clear();
        }
    }
}