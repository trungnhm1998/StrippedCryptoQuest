using System.Collections;
using CryptoQuest.Quest.Actor.Categories;
using DG.Tweening;
using IndiGames.Core.Events.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Menus.Item.UI
{
    public class UIMenuLogger : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private GameObject _panel;
        [SerializeField] private float _delayTime = 1f;
        [SerializeField] private VoidEventChannelSO _clearMenuLoggerEvent;

        private Tween _delayTween;

        private void OnEnable()
        {
            _clearMenuLoggerEvent.EventRaised += HideLoggerPanel;
        }

        private void OnDisable()
        {
            _clearMenuLoggerEvent.EventRaised -= HideLoggerPanel;
        }


        /// <summary>
        /// Set logger text and auto hide the panel after _delayTime
        /// When there's new log, the auto hide is cancel and the hiding time is reset
        /// </summary>
        /// <param name="localizedLog"></param>
        public void SetLoggerText(LocalizedString localizedLog)
        {
            _delayTween?.Kill();
            _panel.SetActive(true);
            StartCoroutine(CoGetAndSetText(localizedLog));

            _delayTween = DOVirtual.DelayedCall(_delayTime, HideLoggerPanel);
        }

        private IEnumerator CoGetAndSetText(LocalizedString localizedLog)
        {
            var handle = localizedLog.GetLocalizedStringAsync();
            yield return handle;
            _text.text += $"{handle.Result}\n";
        }

        private void HideLoggerPanel()
        {
            _panel.SetActive(false);
            _text.text = "";
        }
    }
}