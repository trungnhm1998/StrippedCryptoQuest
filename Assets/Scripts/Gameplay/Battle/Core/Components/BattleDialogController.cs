using CryptoQuest.Events;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components
{
    public class BattleDialogController : MonoBehaviour
    {
        [Header("Listen Events")]
        [SerializeField] private StringEventChannelSO _gotNewLogEventChannel;
        [SerializeField] private VoidEventChannelSO _unitDoneActionEventChannel;
        [SerializeField] private VoidEventChannelSO _endActionPhaseEventChannel;

        [Header("Raise Events")]
        [SerializeField] private PagingDialogEventChannelSO _showPagingDialogEventChannel;


        private PagingDialog _currentDialog = new();
        private DialogPage _currentPage;

        private void Awake()
        {
            OnUnitDoneAction();
        }

        private void OnEnable()
        {
            _gotNewLogEventChannel.EventRaised += OnGotNewLog;
            _unitDoneActionEventChannel.EventRaised += OnUnitDoneAction;
            _endActionPhaseEventChannel.EventRaised += OnEndActionPhase;
        }

        private void OnDisable()
        {
            _gotNewLogEventChannel.EventRaised -= OnGotNewLog;
            _unitDoneActionEventChannel.EventRaised -= OnUnitDoneAction;
            _endActionPhaseEventChannel.EventRaised -= OnEndActionPhase;
        }

        private void OnGotNewLog(string message)
        {
            _currentPage.Lines.Add(message);
        }

        private void OnUnitDoneAction()
        {
            if (_currentPage != null && _currentPage.Lines.Count <= 0) return;
            _currentPage = new DialogPage();
            _currentDialog.Pages.Add(_currentPage);
        }

        private void OnEndActionPhase()
        {
            _currentDialog.Pages.Remove(_currentPage);
            _showPagingDialogEventChannel.RaiseEvent(_currentDialog);
            _currentDialog = new();
            _currentPage = new DialogPage();
            _currentDialog.Pages.Add(_currentPage);
        }
    }
}