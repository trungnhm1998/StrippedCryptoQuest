using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.Inventory.Items;
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
        void Equip(EquipmentInfo equipment);
        void Unequip(EquipmentInfo equipment);

        /// <summary>
        /// Create a <see cref="GameplayEffectSpec"/> using this character <see cref="GameplayAbilitySystem"/>
        /// </summary>
        /// <param name="equipment"></param>
        /// <returns>A gameplay spec that can be use to apply into the system</returns>
        GameplayEffectSpec CreateEffectSpecFromEquipment(EquipmentInfo equipment);
    }

    /// <summary>
    /// Should be a component on scene so that we can use the update
    /// </summary>
    public class CharacterBehaviour : MonoBehaviour, ICharacter
    {
        [SerializeField] private CharacterSpec _spec = new();
        [field: SerializeField] public AbilitySystemBehaviour GameplayAbilitySystem { get; private set; }
        [field: SerializeField] public EffectSystemBehaviour EffectSystem { get; private set; }
        [field: SerializeField] public AttributeSystemBehaviour AttributeSystem { get; private set; }
        public Elemental Element => _spec.Element;
        public CharacterSpec Spec => _spec;
        private IEquipmentEffectApplier _equipmentEffectApplier;
        private IStatInitializer _statsInitializer;

        private void OnValidate()
        {
            if (GameplayAbilitySystem == null) GameplayAbilitySystem = GetComponent<AbilitySystemBehaviour>();
        }

        private void Awake()
        {
            GetDependencies();
        }

        /// <summary>
        /// Then we will need to add stats such as ATK, DEF, etc. these need to init after the base stats so when  <see cref="AttributeScriptableObject.CalculateInitialValue"/> get called, it will have the base stats value
        /// </summary>
        public void Init(CharacterSpec character)
        {
            if (character.IsValid() == false) return;
            GetDependencies();
            _spec = character;
            _spec.Bind(this);
            _statsInitializer.InitStats();
            _equipmentEffectApplier.InitEquipments(this);
        }

        private void GetDependencies()
        {
            if (_equipmentEffectApplier == null) _equipmentEffectApplier = GetComponent<IEquipmentEffectApplier>();
            if (_statsInitializer == null) _statsInitializer = GetComponent<IStatInitializer>();
            Assert.IsNotNull(_equipmentEffectApplier); // even for a monster?
        }

        public void SetSlot(PartySlot partySlot)
        {
            transform.SetParent(partySlot.transform);
        }

        public ActiveEffectSpecification ApplyEffect(GameplayEffectSpec effectSpec)
        {
            return GameplayAbilitySystem.ApplyEffectSpecToSelf(effectSpec);
        }

        public void Equip(EquipmentInfo equipment) => _spec.Equipments.Equip(equipment);

        public void Unequip(EquipmentInfo equipment) => _spec.Equipments.Unequip(equipment);

        public GameplayEffectSpec CreateEffectSpecFromEquipment(EquipmentInfo equipment)
        {
            if (equipment.IsValid() == false) return new GameplayEffectSpec();
            return GameplayAbilitySystem.MakeOutgoingSpec(equipment.EffectDef);
        }
    }
}