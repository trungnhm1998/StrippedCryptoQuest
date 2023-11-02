using System;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.SaveSystem.Sagas.ScriptableObjects
{
    public class StatsSaveConfig : ScriptableObject
    {
        [Serializable]
        public struct AttributeMap
        {
            public string SerializedName;
            public AttributeScriptableObject AttributeToSave;
        }
        [field: SerializeField] public List<AttributeMap> AttributeToSave { get; private set; } = new();
        
        public Dictionary<string, AttributeScriptableObject> AttributeByName { get; private set; } = new();

        private void OnEnable()
        {
            AttributeByName.Clear();
            foreach (var attributeMap in AttributeToSave)
                AttributeByName.Add(attributeMap.SerializedName, attributeMap.AttributeToSave);
        }
    }
}