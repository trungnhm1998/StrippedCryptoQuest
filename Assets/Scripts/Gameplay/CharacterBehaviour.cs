using CryptoQuest.Gameplay.Character;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay
{
    public interface ICharacter
    {
        void Init(CharacterSpec character);
    }

    public class CharacterBehaviour : MonoBehaviour, ICharacter
    {
        [SerializeField] private bool _initOnStart = true; // Maybe remove this later
        [field: SerializeField] public AbilitySystemBehaviour GameplayAbilitySystem { get; set; }
        [SerializeField] private CharacterSpec _spec;
        private AttributeSystemBehaviour _attributeSystem;

        public Elemental Element => _spec.Element;

        private void OnValidate()
        {
            if (GameplayAbilitySystem == null) GameplayAbilitySystem = GetComponent<AbilitySystemBehaviour>();
        }

        private void Awake()
        {
            _attributeSystem = GameplayAbilitySystem.AttributeSystem;
        }

        private void Start()
        {
            if (_initOnStart) Init();
        }

        public void Init(CharacterSpec character)
        {
            _spec = character;
            Init();
        }

        /// <summary>
        /// Then we will need to add stats such as ATK, DEF, etc. these need to init after the base stats so when  <see cref="AttributeScriptableObject.CalculateInitialValue"/> get called, it will have the base stats value
        /// </summary>
        private void Init()
        {
            InitBaseStats();
            InitElementStats();
            _attributeSystem.UpdateAttributeValues(); // Update the current value

            for (int i = 0; i < _attributeSystem.AttributeValues.Count; i++)
            {
                var attributeValue = _attributeSystem.AttributeValues[i];
                _attributeSystem.AttributeValues[i] = attributeValue.Attribute.CalculateInitialValue(attributeValue,
                    _attributeSystem.AttributeValues);
            }

            _attributeSystem.UpdateAttributeValues(); // Update the current value
        }

        /// <summary>
        /// We will need a base stats such as STR, INT, DEX, etc. these need to init first
        /// </summary>
        private void InitBaseStats()
        {
            var attributeDefs = _spec.StatsDef.Attributes;
            var charLvl = _spec.Level;
            for (int i = 0; i < attributeDefs.Length; i++)
            {
                var attributeDef = attributeDefs[i];
                _attributeSystem.AddAttribute(attributeDef.Attribute);
                var baseValueAtLevel = _spec.GetValueAtLevel(charLvl, attributeDef);
                _attributeSystem.SetAttributeBaseValue(attributeDef.Attribute, baseValueAtLevel);
            }
        }

        private void InitElementStats()
        {
            _attributeSystem.AddAttribute(Element.AttackAttribute);
            _attributeSystem.AddAttribute(Element.ResistanceAttribute);
            for (int i = 0; i < Element.Multipliers.Length; i++)
            {
                var elementMultiplier = Element.Multipliers[i];
                _attributeSystem.AddAttribute(elementMultiplier.Attribute);
                _attributeSystem.SetAttributeBaseValue(elementMultiplier.Attribute,
                    elementMultiplier.Value);
            }
        }
    }
}