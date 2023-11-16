using System.Collections;
using CryptoQuest.Language.Settings;
using CryptoQuest.SaveSystem;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    public class LoadLanguageSaga : MonoBehaviour, ILoader
    {
        [SerializeField] private LanguageSettingSO _languageSetting;

        public IEnumerator Load(SaveSystemSO progressionSystem)
        {
            yield break;
        }
    }
}