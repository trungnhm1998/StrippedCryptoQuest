using UnityEngine;
using System.Collections.Generic;
using CryptoQuest.UI.Battle;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.Gameplay.Battle
{
    public class BattleDialogController : MonoBehaviour
    {
        [Header("Listen Events")]
        [SerializeField] private StringEventChannelSO _gotNewLogEventChannel;
        [SerializeField] private VoidEventChannelSO _unitClearLogsEventChannel;
        [SerializeField] private VoidEventChannelSO _endActionPhaseEventChannel;

        [Header("Raise Events")]
        [SerializeField] private PagingDialogEventChannelSO _showPagingDialogEventChannel;


        private PagingDialog _currentDialog = new();
        private DialogPage _currentPage;

        private void Awake()
        {
            OnClearLog();
        }

        private void OnEnable()
        {
            _gotNewLogEventChannel.EventRaised += OnGotNewLog;
            _unitClearLogsEventChannel.EventRaised += OnClearLog;
            _endActionPhaseEventChannel.EventRaised += OnEndActionPhase;
        }

        private void OnDisable()
        {
            _gotNewLogEventChannel.EventRaised -= OnGotNewLog;
            _unitClearLogsEventChannel.EventRaised -= OnClearLog;
            _endActionPhaseEventChannel.EventRaised -= OnEndActionPhase;
        }

        private void OnGotNewLog(string message)
        {
            _currentPage.Lines.Add(message);
        }

        private void OnClearLog()
        {
            _currentPage = new DialogPage();
            _currentDialog.Pages.Add(_currentPage);
        }

        private void OnEndActionPhase()
        {
            _showPagingDialogEventChannel.RaiseEvent(_currentDialog);
            _currentDialog = new();
            OnClearLog();
        }
    }
}