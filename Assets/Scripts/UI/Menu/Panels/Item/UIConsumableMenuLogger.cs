using System.Collections;
using CryptoQuest.Character.Attributes;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Character;
using DG.Tweening;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    public class UIConsumableMenuLogger : MonoBehaviour
    {
        private const string CHARACTER_NAME = "characterName";
        private const string ATTRIBUTE_NAME = "attributeName";
        private const string ATTRIBUTE_VALUE = "attributeValue";

        [SerializeField] private LocalizedString _localizedLogger;
        [SerializeField] private LoggerAttribute _loggerAttribute;
        [SerializeField] private Text _text;
        [SerializeField] private GameObject _panel;
        [SerializeField] private float _delayTime = 1f;

        private Tween _callbackTween;

        private void OnEnable()
        {
            _loggerAttribute.OnAttributeChanged += SetLoggerDescription;
        }

        private void OnDisable()
        {
            _loggerAttribute.OnAttributeChanged -= SetLoggerDescription;
        }

        private void SetLoggerDescription(AttributeSystemBehaviour attributeSystem, AttributeValue attributeValue)
        {
            _callbackTween?.Kill();
            if (attributeSystem.TryGetComponent<CharacterBehaviourBase>(out var characterBehaviour) == false) return;
            _panel.SetActive(true);

            CharacterSpec characterSpec = characterBehaviour.Spec;
            LocalizedString characterName = characterSpec.BackgroundInfo.DetailInformation.LocalizedName;
            LocalizedString attributeName = ((AttributeScriptableObject)attributeValue.Attribute).DisplayName;
            LocalizedString localizedString = _localizedLogger;

            localizedString.Add(CHARACTER_NAME, characterName);
            localizedString.Add(ATTRIBUTE_NAME, attributeName);
            localizedString.Add(ATTRIBUTE_VALUE, new FloatVariable()
            {
                Value = attributeValue.CurrentValue
            });

            StartCoroutine(CoSetText(localizedString));

            _callbackTween = DOVirtual.DelayedCall(_delayTime, HideLoggerDescription);
        }

        private IEnumerator CoSetText(LocalizedString str)
        {
            var handle = str.GetLocalizedStringAsync();
            yield return handle;
            _text.text += $"{handle.Result}\n";
        }


        private void HideLoggerDescription()
        {
            _panel.SetActive(false);
            _text.text = "";
        }
    }
}