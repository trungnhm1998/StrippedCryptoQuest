using System;
using System.Collections.Generic;
using CommandTerminal;
using CryptoQuest.Character.Attributes;
using CryptoQuest.Gameplay.PlayerParty;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using CoreAttributeSO = IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects.AttributeScriptableObject;

namespace CryptoQuest.System.Cheat
{
    public class CharacterAttributeCheats : MonoBehaviour, ICheatInitializer
    {
        [SerializeField] private CoreAttributeSO[] _targetAttributes;

        private Dictionary<string, CoreAttributeSO> _attributeDict = new();

        private void OnValidate()
        {
#if UNITY_EDITOR
            var paths = AssetDatabase.FindAssets("t:AttributeScriptableObject");
            var attributes = new List<CoreAttributeSO>();
            for (var i = 0; i < paths.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(paths[i]);
                var attribute = AssetDatabase.LoadAssetAtPath<CoreAttributeSO>(path);
                attributes.Add(attribute);
            }
            _targetAttributes = attributes.ToArray();
            EditorUtility.SetDirty(this);
#endif
        }

        public void InitCheats()
        {
            Terminal.Shell.AddCommand("set", TriggerModifyAttributeValue, 2, 3,
                "set <attribute_name> <new_value> <character_index>, to modify current value of character in that index");

            foreach (var attribute in _targetAttributes)
            {
                var splitedStrings = attribute.name.Split('.', 2);
                if (splitedStrings.Length <= 1) continue;
                var shortName = splitedStrings[1].Replace('.', '_').ToLower();
                _attributeDict.Add(shortName, attribute);
                Terminal.Autocomplete.Register(shortName);
            }
        }

        private void TriggerModifyAttributeValue(CommandArg[] args)
        {
            var attributeName = args[0].String.ToLower();
            if (!_attributeDict.TryGetValue(attributeName, out var attribute))
            {
                Debug.LogWarning($"Attribute {attributeName} not found");
                return;
            }

            var newValue = args[1].Float;
            var characterIndex = args[2].Int;
            Debug.Log($"Index: {characterIndex}");

            ModifyAttributeValue(attribute, newValue, characterIndex);
        }

        private void ModifyAttributeValue(CoreAttributeSO attribute, float newValue, int characterIndex)
        {
            var party = ServiceProvider.GetService<IPartyController>();
            if (party == null || party.Party == null)
            {
                Debug.LogWarning($"Party not found!");
                return;
            }

            var members = party.Party.Members;
            if (0 > characterIndex || characterIndex >= members.Length || !members[characterIndex].IsValid()) 
            {
                Debug.LogWarning($"Character index is not valid!");
                return;
            }

            var character = members[characterIndex];
            if (!character.IsValid() || character.CharacterComponent == null
                || character.CharacterComponent.AttributeSystem == null)
            {
                Debug.LogWarning($"Character in index {characterIndex} is not valid!");
                return;
            } 

            var attributeSystem = character.CharacterComponent.AttributeSystem;
            if (!attributeSystem.TryGetAttributeValue(attribute, out var currentValue)) return;

            attributeSystem.SetAttributeBaseValue(attribute, newValue);
            Debug.Log($"Updated base value of {character}'s attribute {attribute} to {newValue}");
        }
    }
}