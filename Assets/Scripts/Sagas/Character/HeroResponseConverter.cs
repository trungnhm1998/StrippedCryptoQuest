using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Character;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Sagas.Character
{
    public interface IHeroResponseConverter
    {
        HeroSpec Convert(Objects.Character responseObject);
    }

    public class HeroResponseConverter : MonoBehaviour, IHeroResponseConverter
    {
        [SerializeField] private AttributeSets _attributeSets;
        
        [SerializeField] private List<Elemental> _elements = new();
        [SerializeField] private List<CharacterClass> _classes = new();

        [Tooltip("The order of character's name must match with the order of character's origin")]
        [SerializeField] private List<String> _charNames = new();

        [Tooltip("The order of character's name must match with the order of character's origin")]
        [SerializeField] private List<Origin> _charOrigins = new();

        [SerializeField] private List<ResponseAttributeMap> _attributeMap = new();

        private Dictionary<string, AttributeScriptableObject> _lookupAttribute;
        private FieldInfo[] _fields;

        private void Awake()
        {
            ServiceProvider.Provide<IHeroResponseConverter>(this);
        }

        public HeroSpec Convert(Objects.Character responseObject)
        {
            _lookupAttribute ??= _attributeMap.ToDictionary(map => map.Name.ToLower(), map => map.Attribute);
            _fields ??= typeof(Objects.Character).GetFields();
            var nftHero = new HeroSpec();
            FillCharacterData(responseObject, ref nftHero);
            return nftHero;
        }

        private void FillCharacterData(Objects.Character response, ref HeroSpec nftHero)
        {
            var heroName = response.name;
            nftHero.Id = response.id;
            nftHero.Experience = (float)(response.exp);
            nftHero.Elemental = _elements.FirstOrDefault(element => element.Id == Int32.Parse(response.elementId));
            nftHero.Class = _classes.FirstOrDefault(@class => @class.Id == Int32.Parse(response.classId));
            FillCharacterStats(response, ref nftHero);
            FillRuntimeStats(response, ref nftHero);
            if (string.IsNullOrEmpty(heroName) == false)
                nftHero.Origin =
                    _charOrigins[_charNames.IndexOf(_charNames.FirstOrDefault(origin => origin == heroName))];
        }

        private void FillRuntimeStats(Objects.Character response, ref HeroSpec nftHero)
        {
            var runtimeStats = new List<AttributeWithValue>();
            runtimeStats.Add(new AttributeWithValue(AttributeSets.Health, response.HP));
            runtimeStats.Add(new AttributeWithValue(AttributeSets.Mana, response.MP));
            nftHero.RuntimeStats = runtimeStats;
        }

        private void FillCharacterStats(Objects.Character response, ref HeroSpec nftHero)
        {
            var initialAttributes = new Dictionary<AttributeScriptableObject, CappedAttributeDef>();

            InitializeAttributes(response, ref initialAttributes);
            InitializeRandomValue(response, ref initialAttributes);
            InitializeModifyStats(response, ref initialAttributes);

            var stats = new StatsDef
            {
                MaxLevel = response.maxLv,
                Attributes = initialAttributes.Values.ToArray()
            };

            nftHero.Stats = stats;
        }

        private void InitializeAttributes(Objects.Character response,
            ref Dictionary<AttributeScriptableObject, CappedAttributeDef> initialAttributes)
        {
            foreach (var fieldInfo in _fields)
            {
                var fieldName = fieldInfo.Name.ToLower();
                if (_lookupAttribute.TryGetValue(fieldName, out var attributeSO) == false) continue;
                var value = (float)fieldInfo.GetValue(response);
                if (initialAttributes.TryGetValue(attributeSO, out var def))
                {
                    if (fieldName.Contains("min"))
                        def.MinValue = value;
                    else
                        def.MaxValue = value;
                    initialAttributes[attributeSO] = def;
                }
                else
                {
                    initialAttributes.Add(attributeSO, new CappedAttributeDef(attributeSO)
                    {
                        MinValue = fieldInfo.Name.ToLower().Contains("min") ? value : -1,
                        MaxValue = fieldInfo.Name.ToLower().Contains("max") ? value : -1,
                    });
                }
            }
        }

        private void InitializeRandomValue(Objects.Character response,
            ref Dictionary<AttributeScriptableObject, CappedAttributeDef> initialAttributes)
        {
            foreach (var fieldInfo in _fields)
            {
                var fieldName = fieldInfo.Name.ToLower();
                if (!fieldName.Contains("add")) continue;
                if (_lookupAttribute.TryGetValue(fieldName, out var attributeSO) == false) continue;
                var value = (float)fieldInfo.GetValue(response);
                if (!initialAttributes.TryGetValue(attributeSO, out var def)) continue;
                def.RandomValue = value;
                initialAttributes[attributeSO] = def;
            }
        }

        private void InitializeModifyStats(Objects.Character response,
            ref Dictionary<AttributeScriptableObject, CappedAttributeDef> initialAttributes)
        {
            foreach (var modifyStat in response.modifyStats)
            {
                if (_lookupAttribute.TryGetValue(modifyStat.AttributeName, out var attributeSO) == false) continue;
                if (!initialAttributes.TryGetValue(attributeSO, out var def)) continue;
                def.ModifyValue = modifyStat.Value;
                initialAttributes[attributeSO] = def;
            }
        }
    }
}