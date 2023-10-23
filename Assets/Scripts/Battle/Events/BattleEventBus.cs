using System;
using TinyMessenger;

namespace CryptoQuest.Battle.Events
{
    public static class BattleEventBus
    {
        private static readonly ITinyMessengerHub _messageHub =
            new TinyMessengerHub(new DefaultSubscriberErrorHandler());

        public static TinyMessageSubscriptionToken SubscribeEvent<T>(Action<T> listener)
            where T : BattleEvent =>
            _messageHub.Subscribe<T>(listener);

        public static void UnsubscribeEvent(TinyMessageSubscriptionToken subscriptionToken) =>
            _messageHub.Unsubscribe(subscriptionToken);

        public static void RaiseEvent<T>(T message) where T : BattleEvent => _messageHub.Publish(message);
    }
}