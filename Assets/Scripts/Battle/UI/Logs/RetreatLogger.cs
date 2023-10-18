using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using CryptoQuest.AbilitySystem.Abilities;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace CryptoQuest.Battle.UI.Logs
{
    public class RetreatLogger : MonoBehaviour
    {
        [SerializeField] private UnityEvent<LocalizedString> _presentLoggerEvent;

        [SerializeField] private RetreatAbility _retreatAbility;
        [SerializeField] private LocalizedString _retreatSuccessLog;
        [SerializeField] private LocalizedString _retreatFailLog;

        private void OnEnable()
        {
            _retreatAbility.RetreatedEvent += OnRetreatSuccess;
            _retreatAbility.RetreatFailedEvent += OnRetreatFail;
        }

        private void OnDisable()
        {
            _retreatAbility.RetreatedEvent -= OnRetreatSuccess;
            _retreatAbility.RetreatFailedEvent -= OnRetreatFail;
        }

        private void OnRetreatSuccess(AbilitySystemBehaviour owner)
        {         
            if (!owner.TryGetComponent<Components.Character>(out var character)) return;

            var characterName = character.LocalizedName;
            _retreatSuccessLog.Add(Constants.CHARACTER_NAME, characterName);

            _presentLoggerEvent.Invoke(_retreatSuccessLog);
        }

        private void OnRetreatFail(AbilitySystemBehaviour owner)
        {         
            if (!owner.TryGetComponent<Components.Character>(out var character)) return;

            var characterName = character.LocalizedName;
            _retreatFailLog.Add(Constants.CHARACTER_NAME, characterName);
            
            _presentLoggerEvent.Invoke(_retreatFailLog);
        }
    }
}