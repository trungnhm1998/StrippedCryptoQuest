using CryptoQuest.Battle.Commands;
using CryptoQuest.Character.Attributes;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    [RequireComponent(typeof(AbilitySystemBehaviour))]
    public class Character : MonoBehaviour
    {
        #region GAS

        private AbilitySystemBehaviour _gas;
        public AttributeSystemBehaviour AttributeSystem => _gas.AttributeSystem;
        public EffectSystemBehaviour GameplayEffectSystem => _gas.EffectSystem;
        public AbilitySystemBehaviour AbilitySystem => _gas;

        #endregion

        private Elemental _element;
        public Elemental Element => _element;

        protected virtual void Awake()
        {
            _gas = GetComponent<AbilitySystemBehaviour>();
            _command = new NullCommand(this);
        }

        public void Init(Elemental element)
        {
            _element = element;
            AttributeSystem.Init();

            var components = GetComponents<CharacterComponentBase>();
            foreach (var comp in components)
                comp.Init();
        }

        public ActiveEffectSpecification ApplyEffect(GameplayEffectSpec effectSpec)
        {
            return AbilitySystem.ApplyEffectSpecToSelf(effectSpec);
        }

        public void RemoveEffect(GameplayEffectSpec activeEffectEffectSpec)
        {
            GameplayEffectSystem.RemoveEffect(activeEffectEffectSpec);
        }

        public bool HasTag(TagScriptableObject tagSO) => _gas.TagSystem.HasTag(tagSO);

        private ICommand _command;

        public void SetCommand(ICommand command)
        {
            _command = command;
        }

        public void ExecuteCommand()
        {
            _command.Execute(); // this should not be null
            _command = new NullCommand(this);
        }
    }

    public class NullCommand : ICommand
    {
        private readonly Character _character;

        public NullCommand(Character character)
        {
            _character = character;
        }

        public void Execute()
        {
            Debug.LogWarning($"No command for {_character.gameObject.name}.");
        }
    }
}