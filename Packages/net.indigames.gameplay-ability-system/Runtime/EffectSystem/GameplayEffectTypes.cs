using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.EffectSystem
{
    /// <summary>
    /// Handle that wraps a FGameplayEffectContext or subclass, to allow it to be polymorphic and replicate properly
    ///
    /// <para>
    /// I'm not really getting this class... ported from GameplayEffectTypes.h
    /// maybe for networking and async stuff?
    /// </para>
    /// </summary>
    public class GameplayEffectContextHandle
    {
        private readonly GameplayEffectContext _data;
        public GameplayEffectContext Get() => _data;

        public GameplayEffectContextHandle(GameplayEffectContext data)
        {
            _data = data;
        }

        public bool IsValid() => _data != null;
    }

    /// <summary>
    /// Data structure that stores an instigator and related data, such as positions and targets
    /// Games can subclass this structure and add game-specific information
    /// It is passed throughout effect execution so it is a great place to track transient information about an execution
    /// </summary>
    public class GameplayEffectContext
    {
        private AbilitySystemBehaviour _instigatorAbilitySystemComponent;
        public AbilitySystemBehaviour InstigatorAbilitySystemComponent => _instigatorAbilitySystemComponent;
        private GameObject _instigator;

        public bool IsValid()
        {
            return true;
        }

        public void AddInstigator(GameObject instigator)
        {
            _instigator = instigator;
            _instigatorAbilitySystemComponent = instigator.GetComponent<AbilitySystemBehaviour>();
        }
    }

    public struct GameplayModifierEvaluatedData
    {
        public AttributeScriptableObject Attribute;
        public EAttributeModifierOperationType ModifierOp;
        public float Magnitude;

        public GameplayModifierEvaluatedData(AttributeScriptableObject hp, EAttributeModifierOperationType add,
            int magnitude)
        {
            Attribute = hp;
            ModifierOp = add;
            Magnitude = magnitude;
        }

        public GameplayModifierEvaluatedData Clone()
        {
            return new GameplayModifierEvaluatedData()
            {
                Attribute = Attribute,
                ModifierOp = ModifierOp,
                Magnitude = Magnitude
            };
        }
    }

    public struct GameplayEffectModCallbackData
    {
        public readonly GameplayEffectSpec Spec;
        public readonly GameplayModifierEvaluatedData ModEvaluatedData;
        public readonly AbilitySystemBehaviour Owner;

        public GameplayEffectModCallbackData(GameplayEffectSpec spec, GameplayModifierEvaluatedData modEvaluatedData,
            AbilitySystemBehaviour owner)
        {
            Spec = spec;
            ModEvaluatedData = modEvaluatedData;
            Owner = owner;
        }
    }
}