using CryptoQuest.Battle.Events;
using IndiGames.Core.Events.ScriptableObjects;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Battle.Presenter
{
    public class ShakeUICommand : IPresentCommand
    {
        private readonly VoidEventChannelSO _shakeEventSO;
        private readonly VoidEventChannelSO _shakeCompleteEventSO;

        public ShakeUICommand(VoidEventChannelSO shakeEventSO, VoidEventChannelSO shakeCompleteEventSO)
        {
            _shakeCompleteEventSO = shakeCompleteEventSO;
            _shakeEventSO = shakeEventSO;
        }

        public StateBase GetState() => new ShakeUIState(_shakeEventSO, _shakeCompleteEventSO);
    }

    public class ShakeUIState : StateBase
    {
        private readonly VoidEventChannelSO _shakeEventSO;
        private readonly VoidEventChannelSO _shakeCompleteEventSO;

        public ShakeUIState(VoidEventChannelSO shakeEventSO, VoidEventChannelSO shakeCompleteEventSO)
        {
            _shakeCompleteEventSO = shakeCompleteEventSO;
            _shakeEventSO = shakeEventSO;
        }

        protected override void OnEnter()
        {
            _shakeEventSO.RaiseEvent();
            _shakeCompleteEventSO.EventRaised += OnShakeComplete;
        }

        private void OnShakeComplete()
        {
            _shakeCompleteEventSO.EventRaised -= OnShakeComplete;
            StateMachine.ChangeState(StateMachine.GetNextCommand);
        }

        protected override void OnExit()
        {
            _shakeCompleteEventSO.EventRaised -= OnShakeComplete;
        }
    }

    public class EnemyAttacked : MonoBehaviour
    {
        [SerializeField] private UnityEvent<IPresentCommand> _enqueueCommand;
        [SerializeField] private VoidEventChannelSO _shakeEventSO;
        [SerializeField] private VoidEventChannelSO _shakeCompleteEventSO;
        private TinyMessageSubscriptionToken _shakeEvent;

        private void Awake()
        {
            _shakeEvent = BattleEventBus.SubscribeEvent<ShakeUIEvent>(_ =>
            {
                _enqueueCommand.Invoke(new ShakeUICommand(_shakeEventSO, _shakeCompleteEventSO));
            });
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_shakeEvent);
        }
    }
}