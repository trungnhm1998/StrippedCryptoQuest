using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.PlayerParty;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;
using UnityEngine;
using UnityEngine.Assertions;

namespace CryptoQuest.Gameplay
{
    public interface ICharacter
    {
        void Init(CharacterSpec character);
        void SetSlot(PartySlot partySlot);
        AbilitySystemBehaviour GameplayAbilitySystem { get; }
        EffectSystemBehaviour EffectSystem { get; }
        AttributeSystemBehaviour AttributeSystem { get; }
        CharacterSpec Spec { get; }
        ActiveEffectSpecification ApplyEffect(GameplayEffectSpec effectSpec);
        void UpdateAttributeValues();
    }

    public class CharacterBehaviour : MonoBehaviour, ICharacter
    {
        [SerializeField] private CharacterSpec _spec;

        [SerializeField] private bool _initOnStart = true; // Maybe remove this later
        [field: SerializeField] public AbilitySystemBehaviour GameplayAbilitySystem { get; set; }
        [field: SerializeField] public EffectSystemBehaviour EffectSystem { get; set; }
        [field: SerializeField] public AttributeSystemBehaviour AttributeSystem { get; private set; }

        public Elemental Element => _spec.Element;

        public CharacterSpec Spec => _spec;

        private IEquipmentController _equipmentController;
        private IStatInitializer _statsInitializer;

        private void OnValidate()
        {
            if (GameplayAbilitySystem == null) GameplayAbilitySystem = GetComponent<AbilitySystemBehaviour>();
        }

        private void Awake()
        {
            GetDependencies();
        }

        private void Start()
        {
            if (_initOnStart) Init();
        }

        public void Init(CharacterSpec character)
        {
            GetDependencies();
            _spec = character;
            _spec.Bind(this);
            Init();
        }

        private void GetDependencies()
        {
            if (_equipmentController == null) _equipmentController = GetComponent<IEquipmentController>();
            if (_statsInitializer == null) _statsInitializer = GetComponent<IStatInitializer>();
            Assert.IsNotNull(_equipmentController); // even for a monster?
        }

        public void SetSlot(PartySlot partySlot)
        {
            transform.SetParent(partySlot.transform);
        }

        public ActiveEffectSpecification ApplyEffect(GameplayEffectSpec effectSpec)
        {
            return GameplayAbilitySystem.ApplyEffectSpecToSelf(effectSpec);
        }

        public void UpdateAttributeValues()
        {
            EffectSystem.UpdateAttributeModifiersUsingAppliedEffects();
        }

        /// <summary>
        /// Then we will need to add stats such as ATK, DEF, etc. these need to init after the base stats so when  <see cref="AttributeScriptableObject.CalculateInitialValue"/> get called, it will have the base stats value
        /// </summary>
        private void Init()
        {
            if (_spec.IsValid() == false) return;
            _statsInitializer.InitStats();
            _equipmentController.InitEquipments(this);
        }
    }
}