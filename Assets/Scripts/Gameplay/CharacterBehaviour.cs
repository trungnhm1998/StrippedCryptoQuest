using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.PlayerParty;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay
{
    public interface ICharacter
    {
        void Init(CharacterSpec character);
        void SetSlot(PartySlot partySlot);
        AbilitySystemBehaviour GAS { get; }
        AttributeSystemBehaviour AttributeSystem { get; }
    }

    public class CharacterBehaviour : MonoBehaviour, ICharacter
    {
        [SerializeField] private bool _initOnStart = true; // Maybe remove this later
        [field: SerializeField] public AbilitySystemBehaviour GameplayAbilitySystem { get; set; }
        [SerializeField] private CharacterSpec _spec;
        public AbilitySystemBehaviour GAS => GameplayAbilitySystem;
        public AttributeSystemBehaviour AttributeSystem => GameplayAbilitySystem.AttributeSystem;

        public Elemental Element => _spec.Element;

        private void OnValidate()
        {
            if (GameplayAbilitySystem == null) GameplayAbilitySystem = GetComponent<AbilitySystemBehaviour>();
        }

        private void Start()
        {
            if (_initOnStart) Init();
        }

        public void Init(CharacterSpec character)
        {
            _spec = character;
            _spec.Bind(this);
            Init();
        }

        public void SetSlot(PartySlot partySlot)
        {
            transform.SetParent(partySlot.transform);
        }

        #region Attributes

        /// <summary>
        /// Then we will need to add stats such as ATK, DEF, etc. these need to init after the base stats so when  <see cref="AttributeScriptableObject.CalculateInitialValue"/> get called, it will have the base stats value
        /// </summary>
        private void Init()
        {
            if (_spec.IsValid() == false) return;
            InitBaseStats();
            InitAllAttributes();
            InitElementStats();

            AttributeSystem.UpdateAttributeValues(); // Update the current value
        }

        /// <summary>
        /// We will need a base stats such as STR, INT, DEX, etc. these need to init first
        ///
        /// Use the <see cref="CharacterSpec.StatsDef"/> which contains the base stats to init
        /// </summary>
        private void InitBaseStats()
        {
            var attributeDefs = _spec.StatsDef.Attributes;
            var charLvl = _spec.Level;
            for (int i = 0; i < attributeDefs.Length; i++)
            {
                var attributeDef = attributeDefs[i];
                AttributeSystem.AddAttribute(attributeDef.Attribute);
                var baseValueAtLevel = _spec.GetValueAtLevel(charLvl, attributeDef);
                AttributeSystem.SetAttributeBaseValue(attributeDef.Attribute, baseValueAtLevel);
            }

            AttributeSystem.UpdateAttributeValues();
        }

        private void InitElementStats()
        {
            AttributeSystem.AddAttribute(Element.AttackAttribute);
            AttributeSystem.AddAttribute(Element.ResistanceAttribute);
            for (int i = 0; i < Element.Multipliers.Length; i++)
            {
                var elementMultiplier = Element.Multipliers[i];
                AttributeSystem.AddAttribute(elementMultiplier.Attribute);
                AttributeSystem.SetAttributeBaseValue(elementMultiplier.Attribute,
                    elementMultiplier.Value);
            }

            AttributeSystem.UpdateAttributeValues();
        }

        /// <summary>
        /// Calculate init value for all attributes, HP, MP will be same as MaxHP, MaxMP
        ///
        /// ATK = STR, DEF = VIT, etc.
        /// </summary>
        private void InitAllAttributes()
        {
            for (int i = 0; i < AttributeSystem.AttributeValues.Count; i++)
            {
                var attributeValue = AttributeSystem.AttributeValues[i];
                AttributeSystem.AttributeValues[i] = attributeValue.Attribute.CalculateInitialValue(attributeValue,
                    AttributeSystem.AttributeValues);
            }
        }

        #endregion
    }
}