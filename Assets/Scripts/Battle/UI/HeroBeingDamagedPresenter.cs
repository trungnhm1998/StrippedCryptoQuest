using System.Collections;
using CryptoQuest.Battle.Character;
using CryptoQuest.Character.Attributes;
using CryptoQuest.UI.Common;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using UnityEngine;
using AttributeScriptableObject = IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects.AttributeScriptableObject;

namespace CryptoQuest.Battle.UI
{
    public class HeroBeingDamagedPresenter : MonoBehaviour
    {
        [SerializeField] private AttributeChangeEvent _heroHealthChangeEvent;
        [SerializeField] private UIShakeEffect _uiShakeEffect;

        private void Awake()
        {
            _heroHealthChangeEvent.Changed += ShakeUIWhenHealthDecrease;
        }

        private void OnDestroy()
        {
            _heroHealthChangeEvent.Changed -= ShakeUIWhenHealthDecrease;
        }

        private void ShakeUIWhenHealthDecrease(AttributeScriptableObject attribute, AttributeValue oldVal,
            AttributeValue newVal)
        {
            if (attribute != AttributeSets.Health) return;
            if (newVal.CurrentValue >= oldVal.CurrentValue) return;
            StartCoroutine(CoPresentShakingUIAndDamageDialog(attribute, oldVal, newVal));
        }

        private IEnumerator CoPresentShakingUIAndDamageDialog(
            AttributeScriptableObject attribute,
            AttributeValue oldVal,
            AttributeValue newVal)
        {
            yield return _uiShakeEffect.CoShake();
            var damage = oldVal.CurrentValue - newVal.CurrentValue;
            var damageDialog = $"{damage:0}";
            Debug.Log($"HeroBeingDamagedPresenter: {damageDialog}");
        }
    }
}