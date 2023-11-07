using System;
using System.Collections.Generic;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Abilities.PostNormalAttackPassive;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuestEditor.Gameplay.Gameplay.Abilities
{
    [CreateAssetMenu(menuName = "Crypto Quest/Ability System/Abilities/Ability Asset Mapping Editor",
        fileName = "AbilityAssetMappingEditor")]
    public class AbilityAssetMappingEditor : ScriptableObject
    {
        public List<ElementMap> ElementMaps = new();
        public List<TargetParameterMap> TargetParameterMaps = new();
        public List<EffectTypeMap> EffectTypeMaps = new();
        public List<TargetTypeMap> TargetTypeMaps = new();
        public List<GameEffectMap> GameEffectMaps = new();
        public List<PhysicalEffectMap> PhysicalEffectMaps = new();
        public List<MagicalEffectMap> MagicalEffectMaps = new();
        public List<ConditionalTargetMap> ConditionalTargetMaps = new();
        public List<ConditionalTargetEffectMap> ConditionalTargetEffectMaps = new();
        public List<GameEffectPairMap> EffectPairMaps = new();
        public List<GameEffectMap> BuffEffects = new();
        public List<GameEffectMap> DebuffEffects = new();

       
    }

    public abstract class BaseMap
    {
        public string Id;
    }

    public abstract class BaseMap<T> : BaseMap
    {
        public T Value;
        protected BaseMap() { }
    }

    [Serializable]
    public class ElementMap : BaseMap<Elemental> { }

    [Serializable]
    public class TargetParameterMap : BaseMap<AttributeScriptableObject> { }

    [Serializable]
    public class EffectTypeMap : BaseMap<EEffectType> { }

    [Serializable]
    public class TargetTypeMap : BaseMap<SkillTargetType> { }

    [Serializable]
    public class GameEffectMap : BaseMap<GameplayEffectDefinition> { }

    [Serializable]
    public class PhysicalEffectMap
    {
        public string Id;
        public List<GameEffectMap> GameEffectMaps = new();
    }

    [Serializable]
    public class MagicalEffectMap
    {
        public string Id;
        public List<GameEffectMap> GameEffectMaps = new();
    }

    [Serializable]
    public class ConditionalTypeMap
    {
        public List<string> Ids;
        public PostNormalAttackPassiveBase Value;
    }

    [Serializable]
    public class ConditionalEffectMap : BaseMap<GameEffectMap> { }

    [Serializable]
    public class ConditionalTargetMap
    {
        public string TargetId;
        public List<ConditionalTypeMap> ConditionalTypeMaps = new();
    }

    [Serializable]
    public class ConditionalTargetEffectMap
    {
        public string TargetId;
        public List<GameEffectMap> ConditionalTypeMaps = new();
    }

    [Serializable]
    public class GameEffectPairMap
    {
        public string AttributeId;
        public List<GameplayEffectDefinition> GameplayEffectDefinitions = new();
    }
}