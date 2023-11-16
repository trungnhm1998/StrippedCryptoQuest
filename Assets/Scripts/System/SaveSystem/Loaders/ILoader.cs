using System.Collections;
using CryptoQuest.SaveSystem;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    public interface ILoader
    {
        public IEnumerator Load(SaveSystemSO progressionSystem);
    }
}