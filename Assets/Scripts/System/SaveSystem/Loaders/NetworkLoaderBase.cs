using System;
using System.Collections;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    [Serializable]
    public abstract class NetworkLoader<TActionToDispatch, TActionToWait> : Loader
        where TActionToWait : ActionBase
        where TActionToDispatch : ActionBase, new()
    {
        public override IEnumerator LoadAsync()
        {
            bool fetched = false;
            var token = ActionDispatcher.Bind<TActionToWait>(_ => fetched = true);
            ActionDispatcher.Dispatch(new TActionToDispatch());
            yield return new WaitUntil(() => fetched);
            ActionDispatcher.Unbind(token);
        }
    }
}