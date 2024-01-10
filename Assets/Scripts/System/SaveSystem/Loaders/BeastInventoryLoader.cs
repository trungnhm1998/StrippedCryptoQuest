using System;
using System.Collections;
using CryptoQuest.Actions;
using IndiGames.Core.Events;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    [Serializable]
    public class BeastInventoryLoader : Loader
    {
        public override IEnumerator LoadAsync()
        {
            ActionDispatcher.Dispatch(new FetchProfileBeastAction());
            yield break;
        }
    }
}