using CryptoQuest.Events;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using CryptoQuest.GameHandler;

namespace CryptoQuest.Gameplay.Battle.Core.Components.ActionVFXHandler
{
    public class ActionVFXHandler : MonoBehaviour
    {
        [Header("Listen Events")]
        [SerializeField] private BattleActionDataEventChannelSO _gotActionDataEventChannel;
        [SerializeField] private VoidEventChannelSO _doneActionEventChannel;
        [SerializeField] private VoidEventChannelSO _endActionPhaseEventChannel;

        [Header("Raise Events")]
        [SerializeField] private LocalizedStringEventChannelSO _showBattleDialogEventChannel;
        [SerializeField] private VoidEventChannelSO _showNextMarkEventChannel;

        private IGameHandler<object> _rootHandler = new GameHandler<object>();
        private IGameHandler<object> _currentHandler = new GameHandler<object>();
        private DoneActionHandler _doneActionHandler;

        private void Awake()
        {
            _rootHandler = _currentHandler;
            _doneActionHandler = new DoneActionHandler(this);
            _doneActionHandler.OnDoneAction += _showNextMarkEventChannel.RaiseEvent;
        }

        protected void OnEnable()
        {
            _doneActionEventChannel.EventRaised += OnUnitDoneAction;
            _gotActionDataEventChannel.EventRaised += OnGotActionData;
        }

        protected void OnDisable()
        {
            _doneActionEventChannel.EventRaised -= OnUnitDoneAction;
            _gotActionDataEventChannel.EventRaised -= OnGotActionData;
        }

        private void OnGotActionData(BattleActionDataSO data)
        {
            var handler = new LogAfterVFXHandler(data);
            handler.OnShowBattleDialog += _showBattleDialogEventChannel.RaiseEvent;
            _currentHandler.SetNext(handler);
            _currentHandler = handler;
        }

        private void OnUnitDoneAction()
        {
            _currentHandler.SetNext(_doneActionHandler);
            _rootHandler.Handle();
        }

        public void ResetHandler()
        {
            _currentHandler = new GameHandler<object>();
            _rootHandler = _currentHandler;
        }
    }
}