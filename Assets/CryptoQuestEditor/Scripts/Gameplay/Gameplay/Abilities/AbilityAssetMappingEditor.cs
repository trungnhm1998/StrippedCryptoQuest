using System;
using System.Collections.Generic;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

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
}