using System;
using System.Collections;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    public interface ILoader
    {
        public IEnumerator LoadAsync();
        public void Load();
    }

    [Serializable]
    public abstract class Loader : ILoader
    {
        public virtual IEnumerator LoadAsync()
        {
            yield break;
        }

        public virtual void Load() { }
    }
}