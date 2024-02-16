using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Character;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay;
using CryptoQuest.Sagas.Objects;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.ChangeClass
{
    public class InitializeNewCharacter : MonoBehaviour
    {
        public UnityAction<HeroSpec> HeroSpecEvent;
        [SerializeField] private ChangeClassSyncData _syncData;
        [SerializeField] private List<ResponseAttributeMap> _attributeMap = new();
        private Dictionary<string, AttributeScriptableObject> _lookupAttribute = new();
        private FieldInfo[] _fields;
        private HeroSpec _heroSpec;
        public bool IsFinishFetchData { get; private set; }

        private void Awake()
        {
            _lookupAttribute = _attributeMap.ToDictionary(map => map.Name, map => map.Attribute);
        }

        public void GetStats(API.NewCharacter response)
        {
            _heroSpec = new();
            IsFinishFetchData = false;
            var initialAttributes = new Dictionary<AttributeScriptableObject, CappedAttributeDef>();
            _fields ??= typeof(API.NewCharacter).GetFields();
            foreach (var fieldInfo in _fields)
            {
                if (_lookupAttribute.TryGetValue(fieldInfo.Name, out var attributeSO) == false) continue;
                var value = (float)fieldInfo.GetValue(response);
                if (initialAttributes.TryGetValue(attributeSO, out var def))
                {
                    if (fieldInfo.Name.Contains("min"))
                        def.MinValue = value;
                    else
                        def.MaxValue = value;
                    initialAttributes[attributeSO] = def;
                }
                else
                {
                    initialAttributes.Add(attributeSO, new CappedAttributeDef(attributeSO)
                    {
                        MinValue = fieldInfo.Name.Contains("min") ? value : -1,
                        MaxValue = fieldInfo.Name.Contains("max") ? value : -1
                    });
                }
            }

            InitializeRandomValue(response, ref initialAttributes);

            var stats = new StatsDef
            {
                MaxLevel = 0,
                Attributes = initialAttributes.Values.ToArray()
            };
            _heroSpec.Stats = stats;
            BaseStats(response);
        }

        private void InitializeRandomValue(API.NewCharacter response,
            ref Dictionary<AttributeScriptableObject, CappedAttributeDef> initialAttributes)
        {
            foreach (var fieldInfo in _fields)
            {
                var fieldName = fieldInfo.Name;
                if (!fieldName.Contains("add")) continue;
                if (_lookupAttribute.TryGetValue(fieldName, out var attributeSO) == false) continue;
                var value = (float)fieldInfo.GetValue(response);
                if (!initialAttributes.TryGetValue(attributeSO, out var def)) continue;
                def.RandomValue = value;
                initialAttributes[attributeSO] = def;
            }
        }

        private void BaseStats(API.NewCharacter response)
        {
            _heroSpec.Id = response.id;
            _heroSpec.Experience = response.exp;
            _heroSpec.Stats.MaxLevel = response.maxLv;
            SetOrigin(response);
            GetClass(response);
            GetElement(response);
            IsFinishFetchData = true;
            HeroSpecEvent?.Invoke(_heroSpec);
        }

        private void SetOrigin(API.NewCharacter response)
        {
            foreach (var data in _syncData.Origins)
            {
                if (data.DetailInformation.Id.ToString() == response.characterId)
                    _heroSpec.Origin = data;
            }
        }

        private void GetElement(API.NewCharacter response)
        {
            foreach (var data in _syncData.Elements)
            {
                if (data.Id.ToString() == response.elementId)
                    _heroSpec.Elemental = data;
            }
        }

        private void GetClass(API.NewCharacter response)
        {
            foreach (var data in _syncData.Class)
            {
                if (data.Id.ToString() == response.classId)
                    _heroSpec.Class = data;
            }
        }
    }
}
