using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Skill;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay
{
    public class AbilityController : MonoBehaviour, IAbilityController
    {
        [SerializeField] private SimpleAbilitySO _abilityBase;
        [SerializeField] private List<AbilityEffectMapping> _abilityEffectMappings = new();
        private Dictionary<SkillType, EffectScriptableObject> _skillTypeToEffectDictionary = new();

        private void Awake()
        {
            foreach (var abilityEffectMapping in _abilityEffectMappings)
            {
                _skillTypeToEffectDictionary.Add(abilityEffectMapping.SkillType,
                    abilityEffectMapping.EffectScriptableObject);
            }
        }

        public SimpleAbilitySO CreateAbilityInstance(AbilityData data)
        {
            SimpleAbilitySO abilityInstance = Instantiate(_abilityBase);
            abilityInstance.InitAbilityInfo(data.SkillInfo);

            return abilityInstance;
        }

        public EffectScriptableObject CreateAbilityEffectInstance(SkillType skillType)
        {
            bool isAbleToGetEffectBase = _skillTypeToEffectDictionary.TryGetValue(skillType, out var effectBase);
            if (!isAbleToGetEffectBase)
            {
                Debug.LogError($"SkillType {skillType} is not mapped to any effect");
                return null;
            }

            var effectInstance = Instantiate(effectBase);
            return effectInstance;
        }

        public SimpleAbilitySO InitAbility(AbilityData data)
        {
            var ability = CreateAbilityInstance(data);
            var effect = CreateAbilityEffectInstance(data.SkillInfo.SkillType);
            ability.InitAbilityEffect(effect);
            return ability;
        }
    }

    public interface IAbilityController
    {
        SimpleAbilitySO InitAbility(AbilityData data);
    }

    [Serializable]
    public class AbilityEffectMapping
    {
        public SkillType SkillType;
        public EffectScriptableObject EffectScriptableObject;
    }
}