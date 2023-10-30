using System.Collections;
using CryptoQuest.System.Settings;
using UnityEngine;

namespace CryptoQuest.UI.Title
{
    public class UIOptionPanel : MonoBehaviour
    {
        [SerializeField] private LanguageSettingController languageSettingController;

        public void InitOptionPanel()
        {
            gameObject.SetActive(true);
            StartCoroutine(CoInitialize());
        }

        private IEnumerator CoInitialize()
        {
            yield return true;
            languageSettingController.Initialize();
        }
    }
}