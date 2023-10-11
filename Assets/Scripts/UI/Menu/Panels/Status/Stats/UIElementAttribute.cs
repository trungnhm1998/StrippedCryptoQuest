using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Gameplay;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using TMPro;
using UnityEngine;
using AttributeScriptableObject = IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects.AttributeScriptableObject;

namespace CryptoQuest.UI.Menu.Panels.Status.Stats
{
    public class UIElementAttribute : MonoBehaviour
    {
        [field: SerializeField] public Elemental Elemental { get; set; }
        [SerializeField] private TMP_Text _atkValueText;
        [SerializeField] private TMP_Text _resValueText;

        private AttributeSystemBehaviour _attributeSystem;

        public void SetStats(AttributeSystemBehaviour attributeSystem)
        {
            if (_attributeSystem != null)
            {
                _attributeSystem.PreAttributeChange -= UpdateElementStats;
            }

            _attributeSystem = attributeSystem;
            _attributeSystem.PreAttributeChange += UpdateElementStats;

            _attributeSystem.TryGetAttributeValue(Elemental.AttackAttribute, out var atkValue);
            _attributeSystem.TryGetAttributeValue(Elemental.ResistanceAttribute, out var resValue);

            UpdateElementStats(Elemental.AttackAttribute, atkValue);
            UpdateElementStats(Elemental.ResistanceAttribute, resValue);
        }

        private void UpdateElementStats(AttributeScriptableObject attribute, AttributeValue newValue)
        {
            var text = $"{(int)(newValue.CurrentValue * 100)}%";

            if (attribute == Elemental.AttackAttribute)
            {
                _atkValueText.text = text;
            }
            else if (attribute == Elemental.ResistanceAttribute)
            {
                _resValueText.text = text;
            }
        }
    }
}