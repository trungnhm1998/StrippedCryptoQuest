using System.Collections;
using CryptoQuest.Battle.Events;
using IndiGames.Core.Events.ScriptableObjects;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Battle.Presenter.Commands
{
    public class ShakeUICommand : IPresentCommand
    {
        private readonly VoidEventChannelSO _shakeEventSO;
        private readonly VoidEventChannelSO _shakeCompleteEventSO;
        private bool _shaken;

        public ShakeUICommand(VoidEventChannelSO shakeEventSO, VoidEventChannelSO shakeCompleteEventSO)
        {
            _shakeCompleteEventSO = shakeCompleteEventSO;
            _shakeEventSO = shakeEventSO;
        }

        ~ShakeUICommand()
        {
            _shakeCompleteEventSO.EventRaised -= OnShakeComplete;
        }

        public IEnumerator Present()
        {
            _shakeCompleteEventSO.EventRaised += OnShakeComplete;
            _shakeEventSO.RaiseEvent();
            yield return new WaitUntil(() => _shaken);
        }

        private void OnShakeComplete()
        {
            _shakeCompleteEventSO.EventRaised -= OnShakeComplete;
            _shaken = true;
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