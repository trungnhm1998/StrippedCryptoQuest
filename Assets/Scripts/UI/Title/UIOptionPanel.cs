using System.Collections;
using CryptoQuest.Input;
using CryptoQuest.System.Settings;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Title
{
    public class UIOptionPanel : MonoBehaviour
    {
        [SerializeField] private LanguageController _languageController;

        public void InitOptionPanel()
        {
            gameObject.SetActive(true);
            StartCoroutine(CoInitialize());
        }

        private IEnumerator CoInitialize()
        {
            yield return true;
            _languageController.Initialize();
        }
    }
}