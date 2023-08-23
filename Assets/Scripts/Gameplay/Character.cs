using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay
{
    public class Character : MonoBehaviour
    {
        public AbilitySystemBehaviour GameplayAbilitySystem;
        public Elemental Element;

        private IStatInitializer _statsInitializer;

        private void OnValidate()
        {
            if (GameplayAbilitySystem == null) GameplayAbilitySystem = GetComponent<AbilitySystemBehaviour>();
        }

        private void Awake()
        {
            _statsInitializer = GetComponent<IStatInitializer>();
        }

        private void Start()
        {
            _statsInitializer.InitStats();
            GameplayAbilitySystem.AttributeSystem.AddAttribute(Element.AttackAttribute);
            GameplayAbilitySystem.AttributeSystem.AddAttribute(Element.ResistanceAttribute);
            for (int i = 0; i < Element.Multipliers.Length; i++)
            {
                var elementMultiplier = Element.Multipliers[i];
                GameplayAbilitySystem.AttributeSystem.AddAttribute(elementMultiplier.Attribute);
                GameplayAbilitySystem.AttributeSystem.SetAttributeBaseValue(elementMultiplier.Attribute,
                    elementMultiplier.Value);
            }

            GameplayAbilitySystem.AttributeSystem.UpdateAttributeValues();
        }
    }
}