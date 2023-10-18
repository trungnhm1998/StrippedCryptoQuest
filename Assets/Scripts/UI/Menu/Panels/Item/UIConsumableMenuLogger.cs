using System.Collections;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.AbilitySystem.Attributes.Events;
using CryptoQuest.Battle.Components;
using DG.Tweening;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    public class UIConsumableMenuLogger : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private GameObject _panel;
        [SerializeField] private float _delayTime = 1f;

        private Tween _delayTween;

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