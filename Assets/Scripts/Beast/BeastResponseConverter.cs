using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Character;
using CryptoQuest.Gameplay;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Beast
{
    public interface IBeastResponseConverter
    {
        IBeast Convert(BeastResponse responseObject);
    }

    public class BeastResponseConverter : MonoBehaviour, IBeastResponseConverter
    {
        [SerializeField] private List<Elemental> _elements = new();
        [SerializeField] private List<CharacterClass> _classes = new();
        [SerializeField] private List<BeastTypeSO> _type = new();
        [SerializeField] private List<PassiveAbility> _passive = new();
        [SerializeField] private List<ResponseAttributeMap> _attributeMap = new();
        private Dictionary<string, AttributeScriptableObject> _lookupAttribute = new();
        private FieldInfo[] _fields;

        private void Awake()
        {
            _lookupAttribute = _attributeMap.ToDictionary(map => map.Name, map => map.Attribute);
            ServiceProvider.Provide<IBeastResponseConverter>(this);
        }

        public IBeast Convert(BeastResponse response)
        {
            var beast = new Beast()
            {
                Id = response.id,
                BeastId = response.beastId,
                Level = response.level,
                MaxLevel = response.maxLv,
                Stars = response.star,
                Elemental = _elements.FirstOrDefault(element => element.Id == Int32.Parse(response.elementId)),
                Class = _classes.FirstOrDefault(classes => classes.Id == Int32.Parse(response.classId)),
                Type =
                    _type.FirstOrDefault(type => type.Id == Int32.Parse(response.characterId)),
                Passive = _passive.FirstOrDefault(passive => passive.Id == response.passiveSkillId),
                Stats = FillBeastStats(response)
            };
            return beast;
        }

        private StatsDef FillBeastStats(BeastResponse response)
        {
            var initialAttributes = new Dictionary<AttributeScriptableObject, CappedAttributeDef>();
            _fields ??= typeof(BeastResponse).GetFields();
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

            var stats = new StatsDef
            {
                MaxLevel = response.maxLv,
                Attributes = initialAttributes.Values.ToArray()
            };
            return stats;
        }
    }
}