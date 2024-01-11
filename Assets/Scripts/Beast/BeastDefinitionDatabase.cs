using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Character;
using CryptoQuest.Sagas.Objects;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Beast
{
    public class BeastDefinitionDatabase : ScriptableObject
    {
        [SerializeField] private Elemental[] _elements;
        [SerializeField] private CharacterClass[] _classes;
        [SerializeField] private BeastTypeSO[] _type;
        [SerializeField] private List<ResponseAttributeMap> _attributeMap;
        private FieldInfo[] _fields;

        private Dictionary<int, Elemental> _elementalLookupTable = new();
        private Dictionary<int, CharacterClass> _classLookupTable = new();
        private Dictionary<int, BeastTypeSO> _typeLookupTable = new();
        private Dictionary<string, AttributeScriptableObject> _lookupAttribute = new();

        private void OnEnable()
        {
            _lookupAttribute = _attributeMap.ToDictionary(map => map.Name, map => map.Attribute);

            _elementalLookupTable = _elements.ToDictionary(element => element.Id);
            _classLookupTable = _classes.ToDictionary(classes => classes.Id);
            _typeLookupTable = _type.ToDictionary(type => type.Id);
        }

        public Elemental GetElemental(string id) => _elementalLookupTable[int.Parse(id)];

        public CharacterClass GetClass(string id) => _classLookupTable[int.Parse(id)];

        public BeastTypeSO GetType(string id) => _typeLookupTable[int.Parse(id)];
        public FieldInfo[] GetFields() => _fields ??= typeof(BeastResponse).GetFields();

        public bool TryGetAttribute(string name, out AttributeScriptableObject attribute) =>
            _lookupAttribute.TryGetValue(name, out attribute);
    }
}