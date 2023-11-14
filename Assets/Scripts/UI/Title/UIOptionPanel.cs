using System.Collections;
using CryptoQuest.System.Settings;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.UI.Title
{
    public class UIOptionPanel : MonoBehaviour
    {
        [SerializeField] private UILanguageOptions _languageOptions;

        public void InitOptionPanel()
        {
            gameObject.SetActive(true);
            StartCoroutine(CoInitialize());
        }

        private IEnumerator CoInitialize()
        {
            yield return true;
            _languageOptions.Initialize();
        }
    }
}