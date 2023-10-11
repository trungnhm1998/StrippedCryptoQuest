using System.Collections;
using System.Collections.Generic;
using CryptoQuest.UI.Dialogs.BattleDialog;
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
        private bool _finished = true;
        public bool Finished => _lines.Count == 0 && _finished;

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

        public void AppendLog(LocalizedString message)
        {
            _lines.Enqueue(message);
            if (_finished == false) return;
            StartCoroutine(CoShow());
        }

        private IEnumerator CoShow()
        {
            _finished = false;
            yield return _dialog.WithMessage(_lines.Peek()).CoShow();
            yield return new WaitForSeconds(_delayBetweenLines);
            _lines.Dequeue();
            var count = MAX_LINE;
            while (_lines.Count > 0)
            {
                count--;
                if (count <= 0)
                {
                    count = MAX_LINE;
                    _dialog.Clear();
                }
                yield return _dialog.AppendMessage(_lines.Peek());
                yield return new WaitForSeconds(_delayBetweenLines);
                _lines.Dequeue();
            }

            _finished = true;
        }

        public void Clear()
        {
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