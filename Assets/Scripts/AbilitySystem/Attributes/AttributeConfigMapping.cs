using System;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.AbilitySystem.Attributes
{
    public class AttributeConfigMapping : ScriptableObject
    {
        [Serializable]
        public class Map
        {
            public AttributeScriptableObject Attribute;
            public LocalizedString Name;
        }

        [SerializeField] private Map[] _maps;

        private Dictionary<AttributeScriptableObject, Map> _mapDictionary = new();

        private void OnEnable()
        {
            _mapDictionary = new Dictionary<AttributeScriptableObject, Map>();
            foreach (var map in _maps)
            {
                _mapDictionary.Add(map.Attribute, map);
            }
        }

        public bool TryGetMap(AttributeScriptableObject attribute, out Map map)
        {
            var isSuccess = _mapDictionary.TryGetValue(attribute, out map);
            // preload to prevent bug attribute name not display first time
            map.Name.GetLocalizedStringAsync();
            return isSuccess;
        } 
    }
}