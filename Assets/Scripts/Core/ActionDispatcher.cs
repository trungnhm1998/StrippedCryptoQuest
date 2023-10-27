using System;
using TinyMessenger;

namespace CryptoQuest.Core
{
    public abstract class ActionBase : ITinyMessage
    {
        public object Sender { get; } = null;
    }

    public static class ActionDispatcher
    {
        private static readonly ITinyMessengerHub MessengerHub =
            new TinyMessengerHub(new DefaultSubscriberErrorHandler());

        public static void Dispatch<T>(T action) where T : ActionBase => MessengerHub.Publish(action);

        public static TinyMessageSubscriptionToken Bind<T>(Action<T> callback) where T : ActionBase =>
            MessengerHub.Subscribe<T>(callback);

        public static void Unbind(TinyMessageSubscriptionToken token) => MessengerHub.Unsubscribe(token);
    }
}