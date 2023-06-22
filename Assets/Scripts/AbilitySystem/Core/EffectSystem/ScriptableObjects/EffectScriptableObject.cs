using System;
using System.Collections.Generic;
using UnityEngine;

namespace Indigames.AbilitySystem
{
    public abstract class EffectScriptableObject : ScriptableObject
    {
        [Tooltip("どのような属性で、どのような影響を受けるかを入れる")] [TextArea(0, 3)]
        public string Description;

        [Tooltip("どのような属性で、どのような影響を受けるかを入れる")] public EffectDetails EffectDetails;

        /// <summary>
        /// When the effect is applied to a target, these tags will be granted through logic of SkillSystem
        /// </summary>
        [Tooltip("ターゲットに効果が適用されると、\nこれらのタッグが付与されます。")]
        public TagScriptableObject[] GrantedTags;

        /// <summary>
        /// These tags must be present or must not (ignore tags) for the effect to be applied.
        /// </summary>
        [Tooltip("これらのタッグは、効果が適用されるためには、\n存在しなければならないか、存在してはならない（タッグを無視する）。")]
        public TagRequireIgnoreDetails ApplicationTagRequirements;
        // [Tooltip("効果の[val]を入れる\n[val]%の場合は100を分けてください。\n例えば、2%上がるなら0.02、下がるなら-0.02"), ReadOnly]
        // public float Value;

        // [Tooltip("これは効果の条件や時点など")] public EffectConditionSO Condition;

        [Tooltip("これはどのように効果を計算するかを入れる。デフォールトは「DefaultExecutionCalculation」にしてください。")]
        public AbstractEffectExecutionCalculationSO ExecutionCalculation;

        [Header("Application rules")] [Range(0f, 1f)]
        public float ChanceToApply = 1f;
        
        /// <summary>
        /// -1 means infinite
        /// </summary>
        public int StackLimit = 1;

        public bool RemoveAfterApplied;
        public bool RemoveAtEndCombat;

        public List<EffectCustomApplicationRequirement> CustomApplicationRequirements = new List<EffectCustomApplicationRequirement>();

        /// <summary>
        /// Get a new AbstractEffect instance with the given level and level rate. from this ScriptableObject.
        /// note level and level rate are specific to Mugen Horror Action game.
        /// </summary>
        /// <param name="skillSystem"></param>
        /// <param name="levelRate"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public virtual AbstractEffect GetEffect(SkillSystem skillSystem, object origin, SkillParameters skillParameters)
        {
            // Effect can be stacked, so we need to create a new instance of it.
            // TODO: Implement stacked effect by combine their modifiers with the same type (compare using SO)
            var effect = CreateEffect();
            effect.Origin = origin.ToString();
            effect.InitEffect(this, skillSystem, skillParameters);
            return effect;
        }

        protected abstract AbstractEffect CreateEffect();

        public Action OnEffectApplied;
        public Action OnEffectActivated;
        public Action OnEffectDeactivated;
    }

    public abstract class EffectScriptableObject<T> : EffectScriptableObject where T : AbstractEffect, new()
    {
        protected override AbstractEffect CreateEffect() => new T();
    }
}