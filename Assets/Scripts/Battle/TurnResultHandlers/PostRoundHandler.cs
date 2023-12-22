using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.States.SelectHeroesActions;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle.TurnResultHandlers
{
    /// <summary>
    /// Change <see cref="BattleStateMachine"/> to the next state after all the actions has been presented
    /// </summary>
    public class PostRoundHandler : MonoBehaviour
    {
        [SerializeField] private BattleStateMachine _battleStateMachine;
        private TinyMessageSubscriptionToken _finishedPresentingEvent;

        public bool PreventNextState { get; set; }

        private void Awake()
        {
            _finishedPresentingEvent = BattleEventBus.SubscribeEvent<FinishedPresentingEvent>(_ =>
            {
                if (!PreventNextState) _battleStateMachine.ChangeState(new SelectHeroesActions());
            });
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_finishedPresentingEvent);
        }
    }
}