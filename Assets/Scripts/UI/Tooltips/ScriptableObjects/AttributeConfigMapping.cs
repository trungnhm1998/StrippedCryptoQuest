﻿using System;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Tooltips.ScriptableObjects
{
    public class AttributeConfigMapping : ScriptableObject
    {
        [Serializable]
        public struct Map
        {
            public AttributeScriptableObject Attribute;
            public string ShortName;
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

        public bool TryGetMap(AttributeScriptableObject attribute, out Map map) =>
            _mapDictionary.TryGetValue(attribute, out map);
    }
}